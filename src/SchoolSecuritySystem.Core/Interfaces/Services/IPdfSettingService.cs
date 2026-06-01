using SchoolSecuritySystem.Core.Dtos.Pdf;
using SchoolSecuritySystem.Core.Models;

namespace SchoolSecuritySystem.Core.Interfaces.Services
{
    public interface IPdfSettingService
    {
        Task<Result<List<PdfPasswordDto>>> GetPasswordHistoryAsync();
        Task<Result<bool>> SetNewPasswordAsync(CreatePdfPasswordDto dto);
    }
}