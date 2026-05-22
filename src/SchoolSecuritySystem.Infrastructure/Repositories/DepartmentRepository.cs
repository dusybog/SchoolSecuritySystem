using Microsoft.EntityFrameworkCore;
using SchoolSecuritySystem.Core.Entities;
using SchoolSecuritySystem.Core.Interfaces.Repositories;
using SchoolSecuritySystem.Infrastructure.Data;


namespace SchoolSecuritySystem.Infrastructure.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;

        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<department>> GetAllAsync() => await _context.departments.ToListAsync();

        public async Task<department?> GetByIdAsync(long id) => await _context.departments.FindAsync(id);

        public async Task AddAsync(department entity)
        {
            await _context.departments.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(department entity)
        {
            _context.departments.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(department entity)
        {
            _context.departments.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}