using SchoolSecuritySystem.Core.Entities;
using SchoolSecuritySystem.Core.Interfaces.Repositories;
using SchoolSecuritySystem.Core.Interfaces.Services;
using SchoolSecuritySystem.Core.Dtos.Common;
using SchoolSecuritySystem.Core.Models;

namespace SchoolSecuritySystem.Core.Services
{
    public class SystemAuditLogService : ISystemAuditLogService
    {
        private readonly ISystemAuditLogRepository _repository;

        public SystemAuditLogService(ISystemAuditLogRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<PagedResultDto<system_audit_log>>> GetAuditLogsAsync(int page, int pageSize)
        {
            var (items, totalCount) = await _repository.GetPagedAsync(page, pageSize);

            var pagedResult = new PagedResultDto<system_audit_log>
            {
                Data = items,
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page
            };

            return Result<PagedResultDto<system_audit_log>>.Success(pagedResult);
        }
    }
}