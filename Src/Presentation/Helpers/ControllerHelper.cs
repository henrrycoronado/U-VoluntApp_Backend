namespace U_VoluntApp_Backend.Src.Presentation.Helpers;

using System.Security.Claims;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;

public static class ControllerHelper
{
    public static string GetProfileId(ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException("Token invalido");
    }

    public static string GetRequesterRole(ClaimsPrincipal user, string defaultRole)
    {
        // Get all roles and prioritize higher levels: SuperUser > Admin > Coordinator > Volunteer
        var userRoles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        if (userRoles.Contains(RoleConstants.SuperUserRole, StringComparer.OrdinalIgnoreCase))
        {
            return RoleConstants.SuperUserRole;
        }

        if (userRoles.Contains(RoleConstants.AdminRole, StringComparer.OrdinalIgnoreCase))
        {
            return RoleConstants.AdminRole;
        }

        if (userRoles.Contains(RoleConstants.CoordinatorRole, StringComparer.OrdinalIgnoreCase))
        {
            return RoleConstants.CoordinatorRole;
        }

        if (userRoles.Contains(RoleConstants.VolunteerRole, StringComparer.OrdinalIgnoreCase))
        {
            return RoleConstants.VolunteerRole;
        }

        return defaultRole;
    }

    public static (string RequesterId, string RequesterRole) GetRequesterInfo(
        ClaimsPrincipal user,
        string defaultRole)
    {
        var requesterId = GetProfileId(user);
        var requesterRole = GetRequesterRole(user, defaultRole);
        return (requesterId, requesterRole);
    }

    public static void EnsureRole(ClaimsPrincipal user, params string[] allowedRoles)
    {
        var userRoles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        if (!userRoles.Any() || !userRoles.Any(r => allowedRoles.Any(ar => string.Equals(ar, r, StringComparison.OrdinalIgnoreCase))))
        {
            throw new InvalidOperationException("No tienes permiso para esta operacion");
        }
    }
}
