using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolSecuritySystem.Core.Entities;

namespace SchoolSecuritySystem.Core.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<role>> GetAllAsync();

        //Task<role> GetByIdAsync(int id);
        //Task AddAsync(role entity);
        //Task UpdateAsync(role entity);
        //Task DeleteAsync(role entity);
    }
}