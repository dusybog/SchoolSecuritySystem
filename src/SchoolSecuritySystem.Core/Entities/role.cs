namespace SchoolSecuritySystem.Core.Entities;

public partial class role
{
    public long id { get; set; }

    public string name { get; set; } = null!;

    public string name_zh { get; set; } = null!;

    public virtual ICollection<user_role> user_roles { get; set; } = new List<user_role>();
}
