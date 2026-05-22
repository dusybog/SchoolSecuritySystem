using System;
using System.Collections.Generic;

namespace SchoolSecuritySystem.Core.Entities;

public partial class user_role
{
    public long id { get; set; }

    public string email { get; set; } = null!;

    public long? department_id { get; set; }

    public long role_id { get; set; }

    public virtual department? department { get; set; }

    public virtual role role { get; set; } = null!;
}
