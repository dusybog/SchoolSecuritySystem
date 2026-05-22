using System;
using System.Collections.Generic;

namespace SchoolSecuritySystem.Core.Entities;

public partial class dispatch_select
{
    public long dispatch_id { get; set; }

    public long department_id { get; set; }

    public short status { get; set; }

    public virtual department department { get; set; } = null!;

    public virtual submission_dispatch dispatch { get; set; } = null!;
}
