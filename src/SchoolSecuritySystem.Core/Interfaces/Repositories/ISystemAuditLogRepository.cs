using SchoolSecuritySystem.Core.Entities;

namespace SchoolSecuritySystem.Core.Interfaces.Repositories
{
    public interface ISystemAuditLogRepository
    {
        Task<(IEnumerable<system_audit_log> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
    }
}