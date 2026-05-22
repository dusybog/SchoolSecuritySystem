using System;
using System.Collections.Generic;

namespace SchoolSecuritySystem.Core.Entities;

public partial class submission
{
    public long id { get; set; }

    public string trace_code { get; set; } = null!;

    public string title { get; set; } = null!;

    public string? reporter_name { get; set; }

    public string? reporter_phone { get; set; }

    public long department_id { get; set; }

    public short status { get; set; }

    public string created_by { get; set; } = null!;

    public DateTime created_at { get; set; }

    public sbyte is_deleted { get; set; }

    public virtual department department { get; set; } = null!;

    public virtual ICollection<submission_dispatch> submission_dispatches { get; set; } = new List<submission_dispatch>();

    public virtual ICollection<submission_version> submission_versions { get; set; } = new List<submission_version>();

    public virtual ICollection<submission_workflow> submission_workflows { get; set; } = new List<submission_workflow>();
}
