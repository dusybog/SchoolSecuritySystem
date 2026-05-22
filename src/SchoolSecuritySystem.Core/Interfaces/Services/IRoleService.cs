using SchoolSecuritySystem.Core.Dtos.Role;

namespace SchoolSecuritySystem.Core.Interfaces.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleOptionDto>> GetOptionsAsync();
    }
}

