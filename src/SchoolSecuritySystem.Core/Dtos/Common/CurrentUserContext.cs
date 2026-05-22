using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolSecuritySystem.Core.Dtos.Common
{
    public class CurrentUserContext
    {
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int? DepartmentId { get; set; }
    }
}
