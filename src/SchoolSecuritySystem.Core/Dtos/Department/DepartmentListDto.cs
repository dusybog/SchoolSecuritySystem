using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolSecuritySystem.Core.Dtos.Department
{
    public class DepartmentListDto
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string ContactEmail { get; set; } = null!;
    }
}
