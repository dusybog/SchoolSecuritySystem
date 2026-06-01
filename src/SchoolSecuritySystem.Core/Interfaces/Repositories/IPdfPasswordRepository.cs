using SchoolSecuritySystem.Core.Entities;

namespace SchoolSecuritySystem.Core.Interfaces.Repositories
{
    public interface IPdfPasswordRepository
    {
        Task<List<pdf_password_log>> GetHistoryAsync(int top = 50);
        Task AddAsync(pdf_password_log entity);
    }
}