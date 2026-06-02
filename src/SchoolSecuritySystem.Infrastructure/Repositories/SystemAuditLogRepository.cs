using Microsoft.EntityFrameworkCore;
using SchoolSecuritySystem.Core.Entities;
using SchoolSecuritySystem.Core.Interfaces.Repositories;
using SchoolSecuritySystem.Infrastructure.Data;


namespace SchoolSecuritySystem.Infrastructure.Repositories
{
    public class SystemAuditLogRepository : ISystemAuditLogRepository
    {
        private readonly AppDbContext _context;

        public SystemAuditLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<system_audit_log> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
        {
            var query = _context.Set<system_audit_log>().AsNoTracking();

            int totalCount = await query.CountAsync();

            // 依時間由新到舊排序，並進行分頁
            var items = await query.OrderByDescending(x => x.created_at)
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return (items, totalCount);
        }
    }
}