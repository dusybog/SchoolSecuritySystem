namespace SchoolSecuritySystem.Core.Entities;

public partial class dispatch_log
{
    public long id { get; set; }

    public long dispatch_id { get; set; }

    public string recipient_email { get; set; } = null!;

    public short status { get; set; }

    public string? message { get; set; }

    public string created_by { get; set; } = null!;

    public DateTime created_at { get; set; }

    public virtual submission_dispatch dispatch { get; set; } = null!;
}
