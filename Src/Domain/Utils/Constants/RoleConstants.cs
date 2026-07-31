namespace U_VoluntApp_Core.Src.Domain.Utils.Constants;

using U_VoluntApp_Core.Src.Domain.Utils.Constants;

public static class RoleConstants
{
    public const string VolunteerRole = "Volunteer";
    public const string CoordinatorRole = "Coordinator";
    public const string AdminRole = "Admin";
    public const string SuperUserRole = "SuperUser";

    public static readonly List<string> AllRoles = new()
    {
        VolunteerRole,
        CoordinatorRole,
        AdminRole,
        SuperUserRole,
    };

    public static readonly List<string> ManagementRoles = new()
    {
        AdminRole,
        CoordinatorRole,
        SuperUserRole,
    };

    public static readonly List<string> AdminRoles = new()
    {
        AdminRole,
        SuperUserRole,
    };

    public static readonly List<string> SuperUserRoles = new()
    {
        SuperUserRole,
    };
}
