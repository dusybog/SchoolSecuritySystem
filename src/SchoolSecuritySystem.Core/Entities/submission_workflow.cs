using System;
using System.Collections.Generic;

namespace SchoolSecuritySystem.Core.Entities;

public partial class submission_workflow
{
    public long id { get; set; }

    public long submission_id { get; set; }

    public short previous_status { get; set; }

    public short current_status { get; set; }

    public string? comment { get; set; }

    public string created_by { get; set; } = null!;

    public DateTime created_at { get; set; }

    public virtual submission submission { get; set; } = null!;
}
