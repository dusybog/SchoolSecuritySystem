
using SchoolSecuritySystem.Core.DTOs.Email;


namespace SchoolSecuritySystem.Core.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, List<EmailAttachment> attachments = null, long dispatchId = 0, long departmentId = 0);
    }
}