using System;
using System.Collections.Generic;

namespace SchoolSecuritySystem.Core.Entities;

public partial class submission_version
{
    public long submission_id { get; set; }

    public int version { get; set; }

    public string encrypted_content { get; set; } = null!;

    public string encrypted_dek { get; set; } = null!;

    public string created_by { get; set; } = null!;

    public DateTime created_at { get; set; }

    public short kek_version { get; set; }

    public DateTime key_updated_at { get; set; }

    public virtual submission submission { get; set; } = null!;
}
