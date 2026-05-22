using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSecuritySystem.Core.Constants;
using SchoolSecuritySystem.Core.Dtos.UserRole;
using SchoolSecuritySystem.Core.Interfaces.Services;
using SchoolSecuritySystem.Web.Controllers.Api.Base;


namespace SchoolSecuritySystem.Web.Controllers.Api
{
    [Route("api/userRoles")]
    [Authorize(Roles = AppRoles.Center)]
    public class UserRoleApiController : ApiControllerBase
    {
        private readonly IUserRoleService _userRoleService;
        private readonly IRoleService _roleService;

        public UserRoleApiController(IUserRoleService userRoleService, IRoleService roleService)
        {
            _userRoleService = userRoleService;
            _roleService = roleService;
        }

        /// <summary>
        /// [GET] HOST/api/UserRoles - 角色權限清單
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUserRoles()
        {
            var result = await _userRoleService.GetListAsync();
            return HandleResult(result);
        }

        /// <summary>
        /// [POST] HOST/api/UserRoles - 新增角色權限
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateUserRole([FromBody] CreateUserRoleDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userRoleService.CreateAsync(createDto);
            if (result.IsSuccess)
            {
                return Ok(new { message = "新增角色權限成功" });
            }
            return HandleResult(result);
        }

        /// <summary>
        /// [PUT] HOST/api/UserRoles/{UserRoleId} - 更新角色權限
        /// </summary>
        [HttpPut("{UserRoleId:long}")]
        public async Task<IActionResult> UpdateUserRole(long UserRoleId, [FromBody] UpdateUserRoleDto updateDto)
        {
            var result = await _userRoleService.UpdateAsync(UserRoleId, updateDto);
            if (result.IsSuccess)
            {
                return Ok(new { message = $"更新角色權限 {UserRoleId} 成功" });
            }
            return HandleResult(result);
        }

        /// <summary>
        /// [DELETE] HOST/api/UserRoles/{UserRoleId} - 刪除角色權限
        /// </summary>
        [HttpDelete("{UserRoleId:long}")]
        public async Task<IActionResult> DeleteUserRole(long UserRoleId)
        {
            var result = await _userRoleService.DeleteAsync(UserRoleId);
            if (result.IsSuccess)
            {
                return Ok(new { message = $"刪除角色權限 {UserRoleId} 成功" });
            }
            return HandleResult(result);
        }

        /// <summary>
        /// [GET] HOST/api/UserRoles/options - 角色權限選項清單
        /// </summary>
        [HttpGet("options")]
        public async Task<IActionResult> GetUserRoleOptions()
        {
            // 保持原架構對 IRoleService 的無痛相容
            var options = await _roleService.GetOptionsAsync();
            return Ok(options);
        }
    }
}