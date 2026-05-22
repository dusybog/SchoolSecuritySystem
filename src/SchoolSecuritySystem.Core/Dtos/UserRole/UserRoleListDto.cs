using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolSecuritySystem.Core.Dtos.UserRole
{
    public class UserRoleListDto
    {
        public long Id { get; set; }

        public string Email { get; set; } = null!;

        public long? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }

        public long RoleId { get; set; }
        public string RoleName { get; set; } = null!;
    }
}
