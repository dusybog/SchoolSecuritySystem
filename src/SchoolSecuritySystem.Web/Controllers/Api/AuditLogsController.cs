using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSecuritySystem.Core.Constants;
using SchoolSecuritySystem.Core.Interfaces.Services;
using SchoolSecuritySystem.Web.Controllers.Api.Base;

namespace SchoolSecuritySystem.Web.Controllers.Api
{
    [Route("api/audit_logs")]
    [Authorize]
    public class AuditLogApiController : ApiControllerBase
    {
        private readonly ISystemAuditLogService _auditLogService;

        public AuditLogApiController(ISystemAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [HttpGet]
        [Authorize(Roles = AppRoles.Center)] // 請依您的實際常數調整，例如 AppRoles.CenterDirector
        public async Task<IActionResult> GetAuditLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
            => HandleResult(await _auditLogService.GetAuditLogsAsync(page, pageSize));
    }
}