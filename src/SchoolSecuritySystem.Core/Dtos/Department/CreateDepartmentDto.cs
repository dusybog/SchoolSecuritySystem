using System.ComponentModel.DataAnnotations;

namespace SchoolSecuritySystem.Core.Dtos.Department
{
    public class CreateDepartmentDto
    {
        [Required(ErrorMessage = "部門名稱為必填")]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "部門代碼為必填")]
        [MaxLength(20)]
        public string Code { get; set; } = null!;

        [Required(ErrorMessage = "聯絡信箱為必填")]
        [EmailAddress(ErrorMessage = "信箱格式不正確")]
        public string ContactEmail { get; set; } = null!;
    }
}
