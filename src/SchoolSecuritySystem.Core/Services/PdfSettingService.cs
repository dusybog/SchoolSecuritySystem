using SchoolSecuritySystem.Core.Dtos.Pdf;
using SchoolSecuritySystem.Core.Entities;
using SchoolSecuritySystem.Core.Interfaces.Repositories;
using SchoolSecuritySystem.Core.Interfaces.Services;
using SchoolSecuritySystem.Core.Models;

namespace SchoolSecuritySystem.Core.Services
{
    public class PdfSettingService : IPdfSettingService
    {
        private readonly IPdfPasswordRepository _pdfPasswordRepo;
        private readonly ICurrentUserService _currentUser;

        public PdfSettingService(IPdfPasswordRepository pdfPasswordRepo, ICurrentUserService currentUser)
        {
            _pdfPasswordRepo = pdfPasswordRepo;
            _currentUser = currentUser;
        }

        public async Task<Result<List<PdfPasswordDto>>> GetPasswordHistoryAsync()
        {
            var logs = await _pdfPasswordRepo.GetHistoryAsync();

            var dtos = logs.Select(x => new PdfPasswordDto
            {
                Password = x.password,
                CreatedBy = x.created_by,
                CreatedAt = x.created_at.AddHours(8)
            }).ToList();

            return Result<List<PdfPasswordDto>>.Success(dtos);
        }

        public async Task<Result<bool>> SetNewPasswordAsync(CreatePdfPasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Password))
                return Result<bool>.Failure(Error.Invalid("密碼不可為空。"));

            var newLog = new pdf_password_log
            {
                password = dto.Password.Trim(),
                created_by = _currentUser.Email,
                created_at = DateTime.UtcNow
            };

            await _pdfPasswordRepo.AddAsync(newLog);

            return Result<bool>.Success(true);
        }
    }
}