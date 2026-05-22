namespace SchoolSecuritySystem.Core.Entities;

public partial class department
{
    public long id { get; set; }

    public string name { get; set; } = null!;

    public string code { get; set; } = null!;

    public string contact_email { get; set; } = null!;

    public virtual ICollection<dispatch_select> dispatch_selects { get; set; } = new List<dispatch_select>();

    public virtual ICollection<submission> submissions { get; set; } = new List<submission>();

    public virtual ICollection<user_role> user_roles { get; set; } = new List<user_role>();
}
