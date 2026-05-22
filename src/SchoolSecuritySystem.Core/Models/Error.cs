using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolSecuritySystem.Core.Models
{
    public record Error(string Code, string Message)
    {
        public static Error NotFound(string msg) => new("NotFound", msg);
        public static Error Forbidden(string msg) => new("Forbidden", msg);
        public static Error Conflict(string msg) => new("Conflict", msg);
        public static Error Invalid(string msg) => new("Invalid", msg);
    }
}
