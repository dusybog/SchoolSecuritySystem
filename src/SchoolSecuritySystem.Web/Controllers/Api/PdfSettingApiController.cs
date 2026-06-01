using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSecuritySystem.Core.Constants;
using SchoolSecuritySystem.Core.Dtos.Pdf;
using SchoolSecuritySystem.Core.Interfaces.Services;
using SchoolSecuritySystem.Web.Controllers.Api.Base;

namespace SchoolSecuritySystem.Web.Controllers.Api
{
    [Route("api/pdf")]
    [Authorize]
    public class PdfSettingApiController : ApiControllerBase
    {
        private readonly IPdfSettingService _pdfSettingService;

        public PdfSettingApiController(IPdfSettingService pdfSettingService)
        {
            _pdfSettingService = pdfSettingService;
        }

        // GET: api/pdf/password
        [HttpGet("password")]
        [Authorize(Roles = AppRoles.Center)] // 限制僅校安中心權限可查看
        public async Task<IActionResult> GetPasswordHistory()
        {
            return HandleResult(await _pdfSettingService.GetPasswordHistoryAsync());
        }

        // POST: api/pdf/password
        [HttpPost("password")]
        [Authorize(Roles = AppRoles.Center)] // 限制僅校安中心權限可修改
        public async Task<IActionResult> SetNewPassword([FromBody] CreatePdfPasswordDto createDto)
        {
            var result = await _pdfSettingService.SetNewPasswordAsync(createDto);

            if (result.IsSuccess)
                return Ok(new { message = "PDF密碼更新成功" });

            return HandleResult(result);
        }
    }
}