using SchoolSecuritySystem.Core.Dtos.Role;
using SchoolSecuritySystem.Core.Interfaces.Repositories;
using SchoolSecuritySystem.Core.Interfaces.Services;

namespace SchoolSecuritySystem.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<RoleOptionDto>> GetOptionsAsync()
        {
            var roles = await _roleRepository.GetAllAsync();

            return roles.Select(x => new RoleOptionDto
            {
                Id = x.id,
                Name = x.name_zh
            });
        }
    }
}

