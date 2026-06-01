namespace SchoolSecuritySystem.Core.Entities
{
    public class pdf_password_log
    {
        public long id { get; set; }
        public string password { get; set; } = null!;
        public string created_by { get; set; } = null!;
        public DateTime created_at { get; set; }
    }
}