using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolSecuritySystem.Core.Dtos.Dispatch
{
    public class DispatchSelectDto
    {
        public string DeptName { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;

        public short Status { get; set; }
    }
}
