namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Contract;

using Microsoft.AspNetCore.Identity;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Profile;

public partial class RoleRequest
{
    public long Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string RequesterProfileCode { get; set; } = null!;

    public string RequestedRoleId { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public int? DurationInMonths { get; set; }

    public string StateCode { get; set; } = null!;

    public string? ResolvedByProfileCode { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public virtual Profile RequesterProfile { get; set; } = null!;

    public virtual Profile? ResolvedByProfile { get; set; }

    public virtual IdentityRole? RequestedRole { get; set; }
}
