using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace SchoolSecuritySystem.Core.Dtos.Submission
{
    public class CreateSubmissionDto
    {
        [Required(ErrorMessage = "通報人姓名為必填")]
        public string Reporter { get; set; } = string.Empty;

        [Required(ErrorMessage = "聯絡電話為必填")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "請選擇通報單位")]
        public long Department { get; set; }

        [Required(ErrorMessage = "主分類為必填")]
        public string MainCategory { get; set; } = string.Empty;

        [Required(ErrorMessage = "事件名稱為必填")]
        public string EventName { get; set; } = string.Empty;

        [Required(ErrorMessage = "通報標題為必填")]
        public string Title { get; set; } = string.Empty;

        [Required]
        public JsonElement? basic { get; set; } = new();

        [Required]
        public JsonElement? attachments { get; set; } = new();

        [Required]
        public JsonElement? conditionalData { get; set; } = new();

        [Required]
        public JsonElement? details { get; set; } = new();

        [Required]
        public JsonElement? persons { get; set; } = new();

        [Required]
        public JsonElement? properties { get; set; } = new();
    }
}
