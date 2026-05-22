namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Auth;

using System.ComponentModel.DataAnnotations.Schema;

public class RefreshToken
{
    public int Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string IdentityUserId { get; set; } = null!;

    public string ProfileCode { get; set; } = null!;

    public string TokenHash { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public string? ReplacedByTokenHash { get; set; }

    public string? CreatedByIp { get; set; }

    public string? RevokedByIp { get; set; }

    public string? UserAgent { get; set; }

    public string? ReasonRevoked { get; set; }

    [NotMapped]
    public string PlainToken { get; set; } = string.Empty;
}
