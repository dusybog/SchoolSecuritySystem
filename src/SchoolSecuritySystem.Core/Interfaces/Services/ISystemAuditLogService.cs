using SchoolSecuritySystem.Core.Entities;
using SchoolSecuritySystem.Core.Dtos.Common;
using SchoolSecuritySystem.Core.Models;

namespace SchoolSecuritySystem.Core.Interfaces.Services
{
    public interface ISystemAuditLogService
    {
        Task<Result<PagedResultDto<system_audit_log>>> GetAuditLogsAsync(int page, int pageSize);
    }
}