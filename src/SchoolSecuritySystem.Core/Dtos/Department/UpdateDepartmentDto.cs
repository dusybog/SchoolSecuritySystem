using System.ComponentModel.DataAnnotations;

namespace SchoolSecuritySystem.Core.Dtos.Department
{
    public class UpdateDepartmentDto
    {
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(20)]
        public string? Code { get; set; }

        [EmailAddress(ErrorMessage = "信箱格式不正確")]
        public string? ContactEmail { get; set; }
    }
}
