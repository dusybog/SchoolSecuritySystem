namespace SchoolSecuritySystem.Core.Dtos.Pdf
{
    // 用於 GET 回傳歷史紀錄
    public class PdfPasswordDto
    {
        public string Password { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    // 用於 POST 接收新密碼
    public class CreatePdfPasswordDto
    {
        public string Password { get; set; } = string.Empty;
    }
}