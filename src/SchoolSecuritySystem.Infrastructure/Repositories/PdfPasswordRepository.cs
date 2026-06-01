using Microsoft.EntityFrameworkCore;
using SchoolSecuritySystem.Core.Entities;
using SchoolSecuritySystem.Core.Interfaces.Repositories;
using SchoolSecuritySystem.Infrastructure.Data;

namespace SchoolSecuritySystem.Infrastructure.Repositories
{
    public class PdfPasswordRepository : IPdfPasswordRepository
    {
        private readonly AppDbContext _context;

        public PdfPasswordRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<pdf_password_log>> GetHistoryAsync(int top = 50)
        {
            // 取得最新設定的歷史紀錄 (依時間遞減排序)
            return await _context.pdf_password_logs
                .OrderByDescending(x => x.created_at)
                .Take(top)
                .ToListAsync();
        }

        public async Task AddAsync(pdf_password_log entity)
        {
            await _context.pdf_password_logs.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}