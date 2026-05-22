using System.ComponentModel.DataAnnotations;

namespace SchoolSecuritySystem.Core.Dtos.UserRole
{
    public class UpdateUserRoleDto
    {
        [EmailAddress(ErrorMessage = "Email 格式不正確")]
        public string? Email { get; set; }

        public long? DepartmentId { get; set; }

        public long? RoleId { get; set; }
    }
}
