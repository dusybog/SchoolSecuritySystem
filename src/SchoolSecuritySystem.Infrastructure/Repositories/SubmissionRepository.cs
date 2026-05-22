using Microsoft.EntityFrameworkCore;
using SchoolSecuritySystem.Core.Constants;
using SchoolSecuritySystem.Core.Entities;
using SchoolSecuritySystem.Core.Interfaces.Repositories;
using SchoolSecuritySystem.Core.Interfaces.Services;
using SchoolSecuritySystem.Infrastructure.Data;


namespace SchoolSecuritySystem.Infrastructure.Repositories
{
    public class SubmissionRepository : ISubmissionRepository
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public SubmissionRepository(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        private IQueryable<submission> GetBaseQuery(bool includeDeleted = false)
        {
            var query = _context.submissions.AsQueryable();

            if (!includeDeleted)
            {
                query = query.Where(s => s.is_deleted == 0);
            }

            // 根據登入者角色，自動套用安全過濾網
            switch (_currentUser.Role)
            {
                case AppRoles.CenterDirector:
                case AppRoles.CenterOfficer:
                    // 最高權限：看全校，不加上限
                    break;
                case AppRoles.DepartmentOfficer:
                    // 系所承辦：看自己的，或是屬於自己系所的
                    query = query.Where(s =>
                        s.created_by == _currentUser.Email ||
                        s.department_id == _currentUser.DepartmentId);
                    break;
                case AppRoles.GeneralUser:
                default:
                    // 一般使用者：只能看自己建立的
                    query = query.Where(s => s.created_by == _currentUser.Email);
                    break;
            }

            return query;
        }

        // ==========================================
        // 讀取操作 (Read)
        // ==========================================
        public async Task<submission?> GetByIdAsync(long id)
        {
            return await GetBaseQuery().FirstOrDefaultAsync(s => s.id == id);
        }

        public async Task<submission?> GetEntityWithDetailsAsync(long id)
        {
            return await GetBaseQuery()
              .Include(s => s.submission_versions)
              .Include(s => s.submission_workflows)
              //.Include(s => s.submission_dispatches)
              .FirstOrDefaultAsync(s => s.id == id);
        }

        public async Task<submission_version?> GetVersionAsync(long submissionId, int versionId)
        {
            return await GetBaseQuery()
              .Where(s => s.id == submissionId)
              .SelectMany(s => s.submission_versions)
              .FirstOrDefaultAsync(v => v.version == versionId);
        }

        public async Task<(IEnumerable<submission> Data, int TotalCount)> GetPagedEntitiesAsync(
          string? exactCreatorEmail,
          List<short>? targetStatuses,
          int page,
          int pageSize)
        {
            // 呼叫 GetBaseQuery()，底層已經自動加上權限過濾了
            var query = GetBaseQuery().Include(s => s.department).AsNoTracking();

            // 業務條件 1：指定特定建立者 (例如：只看"我建立的單")
            if (!string.IsNullOrEmpty(exactCreatorEmail))
            {
                query = query.Where(s => s.created_by == exactCreatorEmail);
            }

            // 業務條件 2：指定特定狀態碼
            if (targetStatuses != null && targetStatuses.Any())
            {
                query = query.Where(s => targetStatuses.Contains(s.status));
            }

            int totalCount = await query.CountAsync();
            if (totalCount == 0) return (new List<submission>(), 0);

            var data = await query
              .OrderByDescending(s => s.created_at)
              .Skip((page - 1) * pageSize)
              .Take(pageSize)
              .ToListAsync();

            return (data, totalCount);
        }

        // ==========================================
        // 派發單管理 (Dispatch)
        // ==========================================
        public async Task<IEnumerable<submission_dispatch>> GetDispatchesBySubmissionIdAsync(long submissionId)
        {
            // 取得當前使用者「有權限看見的通報單池」
            var permittedSubmissions = GetBaseQuery();

            return await _context.submission_dispatches
              .Include(d => d.dispatch_selects)
                .ThenInclude(ds => ds.department)
              .Include(d => d.dispatch_logs)
              .Where(d => d.submission_id == submissionId)
      // 🌟 防護：確認這張派發單的主表，存在於使用者的合法池子中
                      .Where(d => permittedSubmissions.Any(s => s.id == d.submission_id))
              .OrderByDescending(d => d.created_at)
              .ToListAsync();
        }

        public async Task<submission_dispatch?> GetDispatchAsync(long submissionId, long dispatchId)
        {
            var permittedSubmissions = GetBaseQuery();

            return await _context.submission_dispatches
              .Include(d => d.dispatch_selects)
                .ThenInclude(s => s.department)
              .Include(d => d.dispatch_logs)
              .Where(d => d.submission_id == submissionId && d.id == dispatchId)
              .Where(d => permittedSubmissions.Any(s => s.id == d.submission_id))
              .FirstOrDefaultAsync();
        }

        // ==========================================
        // 寫入操作 (Write)
        // ==========================================
        public async Task AddAsync(submission entity)
        {
            await _context.submissions.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(submission entity)
        {
            _context.submissions.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetMaxVersionAsync(long submissionId)
        {
            return await _context.submission_versions
              .Where(v => v.submission_id == submissionId)
              .MaxAsync(v => (int?)v.version) ?? 0;
        }

        public async Task AddDispatchAsync(submission_dispatch dispatch)
        {
            await _context.submission_dispatches.AddAsync(dispatch);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDispatchAsync(submission_dispatch entity)
        {
            _context.submission_dispatches.Update(entity);
            await _context.SaveChangesAsync();
        }

        public void DeleteDispatch(submission_dispatch dispatch)
        {
            _context.submission_dispatches.Remove(dispatch);
            _context.SaveChanges();
        }

        public async Task AddDispatchSelectsAsync(IEnumerable<dispatch_select> selects)
        {
            await _context.dispatch_selects.AddRangeAsync(selects);
            await _context.SaveChangesAsync();
        }

        // ==========================================
        // 背景寄信排程專用 (System Level)
        // ==========================================
        public async Task UpdateDispatchStatusAfterEmailSentAsync(long dispatchId, long departmentId, string recipientEmail, bool isSuccess, string messageMsg)
        {
            // 🌟 1. 開啟交易，保證三步操作「同生共死」
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                short finalStatus = isSuccess ? (short)20 : (short)21;
                int maxMessageLength = 250;
                string safeMessage = string.IsNullOrEmpty(messageMsg)
                    ? ""
                    : (messageMsg.Length > maxMessageLength ? messageMsg.Substring(0, maxMessageLength) : messageMsg);

                // 🌟 2. 更新系所寄送狀態 (不再使用 ExecuteUpdate，改用標準追蹤更新，融入同一個 Transaction)
                var selectEntry = await _context.dispatch_selects
                    .FirstOrDefaultAsync(s => s.dispatch_id == dispatchId && s.department_id == departmentId);

                if (selectEntry != null)
                {
                    selectEntry.status = finalStatus;
                }

                // 🌟 3. 寫入寄信歷程 Log
                var log = new dispatch_log
                {
                    dispatch_id = dispatchId,
                    recipient_email = recipientEmail,
                    status = finalStatus,
                    message = safeMessage,
                    created_by = "背景系統",
                    created_at = DateTime.Now
                };
                _context.dispatch_logs.Add(log);

                // 🌟 4. 更新主檔與併發防護
                var dispatchMaster = await _context.submission_dispatches.FindAsync(dispatchId);
                if (dispatchMaster != null)
                {
                    if (isSuccess)
                    {
                        dispatchMaster.success_count += 1;
                    }

                    // 檢查是否還有其他排隊中的信件
                    // 由於 selectEntry 還沒存進資料庫，我們要在這裡排除掉「當下正在處理的這筆」
                    bool hasPending = await _context.dispatch_selects
                        .AnyAsync(s => s.dispatch_id == dispatchId &&
                                       s.status == 10 &&
                                       s.department_id != departmentId);

                    if (!hasPending)
                    {
                        dispatchMaster.status = dispatchMaster.success_count > 0 ? (short)20 : (short)21;
                    }
                }

                // 🌟 5. 一次性安全寫入並提交
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
