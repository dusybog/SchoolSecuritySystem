using System;
using System.Collections.Generic;

namespace SchoolSecuritySystem.Core.Entities;

public partial class submission_dispatch
{
    public long id { get; set; }

    public long submission_id { get; set; }

    public int total_count { get; set; }

    public int success_count { get; set; }

    public short status { get; set; }

    public string? director_sign { get; set; }

    public DateTime? director_sign_at { get; set; }

    public string? officer_sign { get; set; }

    public DateTime? officer_sign_at { get; set; }

    public string created_by { get; set; } = null!;

    public DateTime created_at { get; set; }

    public virtual ICollection<dispatch_log> dispatch_logs { get; set; } = new List<dispatch_log>();

    public virtual ICollection<dispatch_select> dispatch_selects { get; set; } = new List<dispatch_select>();

    public virtual submission submission { get; set; } = null!;
}
