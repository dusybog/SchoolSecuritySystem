using System.Security.Claims;
using SchoolSecuritySystem.Core.Interfaces.Services;

namespace SchoolSecuritySystem.Web.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // 取得當前的 ClaimsPrincipal
        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;


        public string Email
        {
            get
            {
                if (User == null) return string.Empty;

                return User.FindFirst("preferred_username")?.Value
                    ?? User.FindFirst(ClaimTypes.Email)?.Value
                    ?? User.FindFirst(ClaimTypes.Upn)?.Value
                    ?? string.Empty;
            }
        }

        public string Role
        {
            get
            {
                return User?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
            }
        }

        public long DepartmentId
        {
            get
            {
                var deptClaim = User?.FindFirst("DepartmentId")?.Value;
                return long.TryParse(deptClaim, out var deptId) ? deptId : 0;
            }
        }
    }
}