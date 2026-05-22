using SchoolSecuritySystem.Core.Entities;

namespace SchoolSecuritySystem.Core.Interfaces.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<department>> GetAllAsync();
        Task<department?> GetByIdAsync(long id);
        Task AddAsync(department entity);
        Task UpdateAsync(department entity);
        Task DeleteAsync(department entity);
    }
}