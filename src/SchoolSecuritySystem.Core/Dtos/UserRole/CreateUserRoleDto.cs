using System.ComponentModel.DataAnnotations;

namespace SchoolSecuritySystem.Core.Dtos.UserRole
{
    public class CreateUserRoleDto
    {
        [Required(ErrorMessage = "使用者 Email 為必填")]
        [EmailAddress(ErrorMessage = "Email 格式不正確")]
        public string Email { get; set; } = null!;

        public long? DepartmentId { get; set; }

        [Required(ErrorMessage = "必須指定角色")]
        public long RoleId { get; set; }
    }
}
