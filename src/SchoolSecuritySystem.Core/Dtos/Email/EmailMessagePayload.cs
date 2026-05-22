namespace SchoolSecuritySystem.Core.DTOs.Email
{
    public class EmailMessagePayload
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<EmailAttachment> Attachments { get; set; }
        public long DispatchId { get; set; }
        public long DepartmentId { get; set; }
    }
}