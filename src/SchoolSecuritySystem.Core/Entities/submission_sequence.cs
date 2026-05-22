using System;
using System.Collections.Generic;

namespace SchoolSecuritySystem.Core.Entities;

public partial class submission_sequence
{
    public string date_part { get; set; } = null!;

    public int sequence { get; set; }
}
