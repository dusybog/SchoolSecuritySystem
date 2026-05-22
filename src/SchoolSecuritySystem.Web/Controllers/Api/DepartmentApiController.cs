using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSecuritySystem.Core.Constants;
using SchoolSecuritySystem.Core.Dtos.Department;
using SchoolSecuritySystem.Core.Interfaces.Services;
using SchoolSecuritySystem.Web.Controllers.Api.Base;

namespace SchoolSecuritySystem.Web.Controllers.Api
{
    [Route("api/departments")]
    [Authorize]
    public class DepartmentApiController : ApiControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentApiController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        [Authorize(Roles = AppRoles.Center)]
        public async Task<IActionResult> GetDepartments() => HandleResult(await _departmentService.GetListAsync());

        [HttpPost]
        [Authorize(Roles = AppRoles.Center)]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDto createDto)
        {
            var result = await _departmentService.CreateAsync(createDto);
            if (result.IsSuccess) return Ok(new { message = "新增部門成功", departmentId = result.Value });
            return HandleResult(result);
        }

        [HttpPut("{departmentId:int}")]
        [Authorize(Roles = AppRoles.Center)]
        public async Task<IActionResult> UpdateDepartment(int departmentId, [FromBody] UpdateDepartmentDto updateDto)
        {
            var result = await _departmentService.UpdateAsync(departmentId, updateDto);
            if (result.IsSuccess) return Ok(new { message = $"更新部門 {departmentId} 成功" });
            return HandleResult(result);
        }

        [HttpDelete("{departmentId:int}")]
        [Authorize(Roles = AppRoles.Center)]
        public async Task<IActionResult> DeleteDepartment(int departmentId)
        {
            var result = await _departmentService.DeleteAsync(departmentId);
            if (result.IsSuccess) return Ok(new { message = $"刪除部門 {departmentId} 成功" });
            return HandleResult(result);
        }

        [HttpGet("options")]
        public async Task<IActionResult> GetDepartmentOptions() => HandleResult(await _departmentService.GetOptionsAsync());
    }
}