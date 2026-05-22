using SchoolSecuritySystem.Core.Entities;

namespace SchoolSecuritySystem.Core.Interfaces.Repositories
{
    public interface IUserRoleRepository
    {
        Task<IEnumerable<user_role>> GetAllAsync();
        Task<user_role?> GetByIdAsync(long id);
        Task AddAsync(user_role entity);
        Task UpdateAsync(user_role entity);
        Task DeleteAsync(user_role entity);
        Task<user_role?> GetByEmailWithDetailsAsync(string email);
    }
}