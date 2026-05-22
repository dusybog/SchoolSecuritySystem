using Microsoft.EntityFrameworkCore;
using SchoolSecuritySystem.Core.Entities;
using SchoolSecuritySystem.Core.Interfaces.Repositories;
using SchoolSecuritySystem.Infrastructure.Data;


namespace SchoolSecuritySystem.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<role>> GetAllAsync()
        {
            return await _context.roles.ToListAsync();
        }

        //public async Task<role> GetByIdAsync(int id)
        //{
        //    return await _context.roles.FindAsync(id);
        //}

        //public async Task AddAsync(role entity)
        //{
        //    await _context.roles.AddAsync(entity);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task UpdateAsync(role entity)
        //{
        //    _context.roles.Update(entity);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task DeleteAsync(role entity)
        //{
        //    _context.roles.Remove(entity);
        //    await _context.SaveChangesAsync();
        //}
    }
}