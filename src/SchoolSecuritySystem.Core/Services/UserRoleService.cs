using SchoolSecuritySystem.Core.Dtos.UserRole;
using SchoolSecuritySystem.Core.Entities;
using SchoolSecuritySystem.Core.Interfaces.Repositories;
using SchoolSecuritySystem.Core.Interfaces.Services;
using SchoolSecuritySystem.Core.Models;
using Microsoft.Extensions.Caching.Memory;


namespace SchoolSecuritySystem.Core.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMemoryCache _memoryCache;

        public UserRoleService(IUserRoleRepository userRoleRepository, IMemoryCache memoryCache)
        {
            _userRoleRepository = userRoleRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result<IEnumerable<UserRoleListDto>>> GetListAsync()
        {
            var entities = await _userRoleRepository.GetAllAsync();

            var dtos = entities.Select(x => new UserRoleListDto
            {
                Id = x.id,
                Email = x.email,
                DepartmentId = x.department_id,
                DepartmentName = x.department?.name,
                RoleId = x.role_id,
                RoleName = x.role?.name ?? string.Empty
            }).ToList();

            return Result<IEnumerable<UserRoleListDto>>.Success(dtos);
        }

        public async Task<Result<bool>> CreateAsync(CreateUserRoleDto createDto)
        {
            var newEntity = new user_role
            {
                email = createDto.Email,
                department_id = createDto.DepartmentId,
                role_id = createDto.RoleId
            };

            await _userRoleRepository.AddAsync(newEntity);
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> UpdateAsync(long userRoleId, UpdateUserRoleDto updateDto)
        {
            var userRole = await _userRoleRepository.GetByIdAsync(userRoleId);
            if (userRole == null)
            {
                return Result<bool>.Failure(Error.NotFound($"找不到 ID 為 {userRoleId} 的使用者角色設定檔"));
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Email))
            {
                userRole.email = updateDto.Email;
            }

            if (updateDto.DepartmentId.HasValue)
            {
                userRole.department_id = updateDto.DepartmentId.Value;
            }

            if (updateDto.RoleId.HasValue)
            {
                userRole.role_id = updateDto.RoleId.Value;
            }

            await _userRoleRepository.UpdateAsync(userRole);

            _memoryCache.Remove($"UserStatus_{userRole.email}");

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteAsync(long userRoleId)
        {
            var userRole = await _userRoleRepository.GetByIdAsync(userRoleId);
            if (userRole == null)
            {
                return Result<bool>.Failure(Error.NotFound($"找不到 ID 為 {userRoleId} 的使用者角色設定檔"));
            }

            await _userRoleRepository.DeleteAsync(userRole);

            _memoryCache.Remove($"UserStatus_{userRole.email}");

            return Result<bool>.Success(true);
        }
    }
}