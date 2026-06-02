using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSecuritySystem.Core.Entities
{
    [Table("system_audit_log")]
    public class system_audit_log
    {
        [Key]
        public long id { get; set; }

        // --- 1. 誰 (Who) ---
        [MaxLength(64)]
        public string? user_email { get; set; }
        [MaxLength(64)]
        public string? ip_address { get; set; }

        // --- 2. 做了什麼 (What) ---
        [MaxLength(10)]
        public string http_method { get; set; } = null!; // GET, POST, PUT, DELETE
        [MaxLength(256)]
        public string request_path { get; set; } = null!; // 路由
        public string? request_payload { get; set; } // 請求內容 (如 JSON Body，敏感資料需遮蔽)

        // --- 3. 結果 (Result) ---
        public int status_code { get; set; } // HTTP 狀態碼 (200=成功, 403=禁止, 500=失敗)
        public string? error_message { get; set; } // 錯誤堆疊或訊息

        // --- 4. 何時 (When) ---
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        // --- 5. 防竄改驗證 (Immutability) ---
        [MaxLength(64)]
        public string record_hash { get; set; } = null!; // 該筆資料的 SHA-256 雜湊值

        public int key_version { get; set; }
    }
}