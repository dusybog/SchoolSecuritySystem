using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace SchoolSecuritySystem.Core.Dtos.Submission
{
    public class PatchSubmissionDto
    {
        // 審核動作：例如 "Save" (純儲存不推進狀態), "Approve" (核准), "Reject" (退回)
        [Required]
        public string Action { get; set; } = string.Empty;

        // 審核意見 (退回時必填，核准時選填)
        public string? Comment { get; set; }

        // 前端解析出來的最新表單完整 JSON 資料
        public CreateSubmissionDto? FormData { get; set; }
    }
}
