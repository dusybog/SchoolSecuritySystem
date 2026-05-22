using Microsoft.EntityFrameworkCore;
using SchoolSecuritySystem.Core.Entities;
using SchoolSecuritySystem.Core.Interfaces.Repositories;
using SchoolSecuritySystem.Infrastructure.Data;

namespace SchoolSecuritySystem.Infrastructure.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly AppDbContext _context;

        public UserRoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<user_role>> GetAllAsync()
        {
            return await _context.user_roles
                .Include(ur => ur.department)
                .Include(ur => ur.role)
                .ToListAsync();
        }

        public async Task<user_role?> GetByIdAsync(long id)
        {
            return await _context.user_roles.FindAsync(id);
        }

        public async Task AddAsync(user_role entity)
        {
            await _context.user_roles.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(user_role entity)
        {
            _context.user_roles.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(user_role entity)
        {
            _context.user_roles.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<user_role?> GetByEmailWithDetailsAsync(string email)
        {
            return await _context.user_roles
                .Include(ur => ur.role)
                .Include(ur => ur.department)
                .FirstOrDefaultAsync(ur => ur.email == email);
        }
    }
}