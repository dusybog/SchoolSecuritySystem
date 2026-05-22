using SchoolSecuritySystem.Core.DTOs.Email;
using SchoolSecuritySystem.Core.Interfaces.Services;


namespace SchoolSecuritySystem.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailTaskQueue _taskQueue;

        public EmailService(IEmailTaskQueue taskQueue)
        {
            _taskQueue = taskQueue;
        }

        public async Task SendEmailAsync(string to, string subject, string body, List<EmailAttachment> attachments = null, long dispatchId = 0, long departmentId = 0)
        {
            var payload = new EmailMessagePayload
            {
                ToEmail = to,
                Subject = subject,
                Body = body,
                Attachments = attachments,
                DispatchId = dispatchId,     // 紀錄派發單 ID
                DepartmentId = departmentId  // 紀錄系所 ID
            };
            await _taskQueue.QueueEmailAsync(payload);
        }
    }
}