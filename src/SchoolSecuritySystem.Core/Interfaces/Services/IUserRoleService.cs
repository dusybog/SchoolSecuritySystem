using SchoolSecuritySystem.Core.Dtos.UserRole;
using SchoolSecuritySystem.Core.Models;

namespace SchoolSecuritySystem.Core.Interfaces.Services
{
    public interface IUserRoleService
    {
        Task<Result<IEnumerable<UserRoleListDto>>> GetListAsync();
        Task<Result<bool>> CreateAsync(CreateUserRoleDto createDto);
        Task<Result<bool>> UpdateAsync(long userRoleId, UpdateUserRoleDto updateDto);
        Task<Result<bool>> DeleteAsync(long userRoleId);
    }
}