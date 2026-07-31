namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Auth;

using System;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Profile;

public class UserSecurityAudit
{
    public int Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string ProfileCode { get; set; } = null!;

    public string LastIpAddress { get; set; } = null!;

    public string DeviceFingerprint { get; set; } = null!;

    public DateTime? LastCodeSentAt { get; set; }

    public bool IsTrusted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Profile Profile { get; set; } = null!;
}
