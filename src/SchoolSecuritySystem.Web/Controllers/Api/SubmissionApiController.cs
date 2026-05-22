using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSecuritySystem.Core.Constants;
using SchoolSecuritySystem.Core.Dtos.Dispatch;
using SchoolSecuritySystem.Core.Dtos.Submission;
using SchoolSecuritySystem.Core.Interfaces.Services;
using SchoolSecuritySystem.Web.Controllers.Api.Base;

namespace SchoolSecuritySystem.Web.Controllers.Api
{
    [Route("api/submissions")]
    [ApiController]
    [Authorize]
    public class SubmissionApiController : ApiControllerBase   
    {
        private readonly ISubmissionService _submissionService;
        private readonly IWebHostEnvironment _env; // 🌟 只有最外層的 Controller 認識環境變數

        public SubmissionApiController(ISubmissionService submissionService, IWebHostEnvironment env)
        {
            _submissionService = submissionService;
            _env = env;
        }

        /// <summary>
        /// [GET] HOST/api/submissions?type={history,audit,mail,all}&page=1&pageSize=10 - 表單清單
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSubmissions(
            [FromQuery] string type = "All",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _submissionService.GetPagedListAsync(type, page, pageSize);
            return HandleResult(result);
        }

        /// <summary>
        /// [GET] HOST/api/submissions/{submissionId} - 表單主要資料
        /// </summary>
        [HttpGet("{submissionId:long}")]
        public async Task<IActionResult> GetSubmission(long submissionId)
        {
            var result = await _submissionService.GetDetailAsync(submissionId);
            return HandleResult(result);
        }

        /// <summary>
        /// [POST] HOST/api/submissions - 新增表單
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateSubmission([FromBody] CreateSubmissionDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _submissionService.CreateAsync(createDto);

            // 客製化成功回傳格式以相容舊版前端
            if (result.IsSuccess)
            {
                return Ok(new { message = "通報成功", data = new { id = result.Value } });
            }

            return HandleResult(result);
        }

        /// <summary>
        /// [PATCH] HOST/api/submissions/{submissionId} - 編輯表單(審核、修改)
        /// </summary>
        [HttpPatch("{submissionId:long}")]
        [Authorize(Roles = AppRoles.Auditor)]
        public async Task<IActionResult> UpdateAndAuditSubmission(long submissionId, [FromBody] PatchSubmissionDto dto)
        {
            var result = await _submissionService.AuditAndEditAsync(submissionId, dto);
            return HandleResult(result);
        }

        /// <summary>
        /// [GET] HOST/api/submissions/{submissionId}/versions/{versionId} - 表單詳細資料
        /// </summary>
        [HttpGet("{submissionId:long}/versions/{versionId:int}")]
        public async Task<IActionResult> GetSubmissionVersion(long submissionId, int versionId)
        {
            var result = await _submissionService.GetVersionDetailAsync(submissionId, versionId);
            return HandleResult(result);
        }

        /// <summary>
        /// [GET] HOST/api/submissions/{submissionId}/dispatches - 取得寄信批次清單
        /// </summary>
        [HttpGet("{submissionId:long}/dispatches")]
        [Authorize(Roles = AppRoles.Center)]
        public async Task<IActionResult> GetDispatches(long submissionId)
        {
            var result = await _submissionService.GetDispatchesAsync(submissionId);
            return HandleResult(result);
        }

        /// <summary>
        /// [POST] HOST/api/submissions/{submissionId}/dispatches - 新增寄信批次 (產生空白單)
        /// </summary>
        [HttpPost("{submissionId:long}/dispatches")]
        [Authorize(Roles = AppRoles.Center)]
        public async Task<IActionResult> CreateDispatch(long submissionId)
        {
            var result = await _submissionService.CreateDispatchAsync(submissionId);

            if (result.IsSuccess) return StatusCode(201); // 201 Created
            return HandleResult(result);
        }

        /// <summary>
        /// [PATCH] HOST/api/submissions/{submissionId}/dispatches/{dispatchId}/sign - 寄信批次簽章
        /// </summary>
        [HttpPatch("{submissionId:long}/dispatches/{dispatchId:long}/sign")]
        [Authorize(Roles = AppRoles.Center)]
        public async Task<IActionResult> SignDispatch(long submissionId, long dispatchId, [FromBody] SignDispatchDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { message = "簽章姓名與角色不可為空" });

            var result = await _submissionService.SignDispatchAsync(submissionId, dispatchId, dto);

            if (result.IsSuccess) return Ok(new { message = "簽核成功" });
            return HandleResult(result);
        }

        /// <summary>
        /// [PATCH] HOST/api/submissions/{submissionId}/dispatches/{dispatchId}/select - 寄信批次選擇收件系所
        /// </summary>
        [HttpPatch("{submissionId:long}/dispatches/{dispatchId:long}/select")]
        [Authorize(Roles = AppRoles.Center)]
        public async Task<IActionResult> SelectDispatchDepartments(long submissionId, long dispatchId, [FromBody] List<int> departmentIds)
        {
            if (departmentIds == null || !departmentIds.Any())
                return BadRequest(new { message = "請至少選擇一個系所" });

            var result = await _submissionService.SelectDispatchDepartmentsAsync(submissionId, dispatchId, departmentIds);

            if (result.IsSuccess) return Ok(new { message = "系所選擇成功" });
            return HandleResult(result);
        }

        /// <summary>
        /// [DELETE] HOST/api/submissions/{submissionId}/dispatches/{dispatchId} - 刪除寄信批次
        /// </summary>
        [HttpDelete("{submissionId:long}/dispatches/{dispatchId:long}")]
        [Authorize(Roles = AppRoles.Center)]
        public async Task<IActionResult> DeleteDispatch(long submissionId, long dispatchId)
        {
            var result = await _submissionService.DeleteDispatchAsync(submissionId, dispatchId);

            if (result.IsSuccess) return NoContent(); // 204 No Content
            return HandleResult(result);
        }

        /// <summary>
        /// [POST] HOST/api/submissions/{submissionId}/dispatches/{dispatchId}/preview - 預覽派發單
        /// </summary>
        [HttpPost("{submissionId:long}/dispatches/{dispatchId:long}/preview")]
        [Authorize(Roles = AppRoles.Center)]
        public async Task<IActionResult> PreviewDispatchReport(long submissionId, long dispatchId)
        {
            var result = await _submissionService.GetDispatchReportPdfAsync(submissionId, dispatchId, _env.WebRootPath);

            // 客製化：成功時回傳 FileResult 而不是 JSON
            if (result.IsSuccess)
            {
                return File(result.Value!, "application/pdf");
            }

            // 失敗時交給基礎類別解析錯誤代碼
            return HandleResult(result);
        }

        /// <summary>
        /// [POST] HOST/api/submissions/{submissionId}/dispatches/{dispatchId}/send - 執行寄信批次
        /// </summary>
        [HttpPost("{submissionId:long}/dispatches/{dispatchId:long}/send")]
        [Authorize(Roles = AppRoles.Center)]
        public async Task<IActionResult> SendDispatch(long submissionId, long dispatchId)
        {
            var result = await _submissionService.SendDispatchEmailsAsync(submissionId, dispatchId, _env.WebRootPath);

            // 客製化：背景排程觸發成功回傳 202 Accepted
            if (result.IsSuccess)
            {
                return Accepted(new { success = true, message = "派發單已成功加入背景寄送排程！" });
            }

            // 為了相容前端原本讀取 JSON { success = false, message = "..." } 的格式
            // 這裡可以選擇不使用 HandleResult，而是手動轉換
            var statusCode = result.Error.Code switch
            {
                "NotFound" => 404,
                "Forbidden" => 403,
                _ => 400
            };

            return StatusCode(statusCode, new { success = false, message = result.Error.Message });
        }
    }
}