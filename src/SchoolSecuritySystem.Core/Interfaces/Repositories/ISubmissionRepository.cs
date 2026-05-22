using SchoolSecuritySystem.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolSecuritySystem.Core.Interfaces.Repositories
{
    public interface ISubmissionRepository
    {
        // ==========================================
        // 讀取操作 (Read)
        // ==========================================
        Task<submission?> GetByIdAsync(long id);

        Task<submission?> GetEntityWithDetailsAsync(long id);

        Task<submission_version?> GetVersionAsync(long submissionId, int versionId);

        Task<(IEnumerable<submission> Data, int TotalCount)> GetPagedEntitiesAsync(
            string? exactCreatorEmail,
            List<short>? targetStatuses,
            int page,
            int pageSize);

        // ==========================================
        // 寫入操作 (Write)
        // ==========================================
        Task AddAsync(submission entity);

        Task UpdateAsync(submission entity);

        Task<int> GetMaxVersionAsync(long submissionId);

        // ==========================================
        // 派發單管理 (Dispatch)
        // ==========================================
        Task<IEnumerable<submission_dispatch>> GetDispatchesBySubmissionIdAsync(long submissionId);

        Task<submission_dispatch?> GetDispatchAsync(long submissionId, long dispatchId);

        Task AddDispatchAsync(submission_dispatch dispatch);

        Task UpdateDispatchAsync(submission_dispatch entity);

        void DeleteDispatch(submission_dispatch dispatch);

        Task AddDispatchSelectsAsync(IEnumerable<dispatch_select> selects);

        Task UpdateDispatchStatusAfterEmailSentAsync(long dispatchId, long departmentId, string recipientEmail, bool isSuccess, string messageMsg);
    }
}