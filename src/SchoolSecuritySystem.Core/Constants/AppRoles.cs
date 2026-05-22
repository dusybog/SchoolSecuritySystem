namespace SchoolSecuritySystem.Core.Constants
{
    public static class AppRoles
    {
        public const string CenterDirector = "CenterDirector";
        public const string CenterOfficer = "CenterOfficer";
        public const string DepartmentOfficer = "DepartmentOfficer";
        public const string GeneralUser = "GeneralUser";

        public const string Center = $"{CenterDirector},{CenterOfficer}";
        public const string Auditor = $"{Center},{DepartmentOfficer}";
    }
}