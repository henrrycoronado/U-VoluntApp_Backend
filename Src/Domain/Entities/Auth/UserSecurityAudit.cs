namespace U_VoluntApp_Core.Src.Domain.Entities.Auth;

using System;

public class UserSecurityAudit
{
    public string UvaCode { get; private set; } = string.Empty;

    public string ProfileCode { get; private set; } = string.Empty;

    public string LastIpAddress { get; private set; } = string.Empty;

    public string DeviceFingerprint { get; private set; } = string.Empty;

    public DateTime? LastCodeSentAt { get; private set; }

    public bool IsTrusted { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public static UserSecurityAudit Create(string profileCode, string ip, string fingerprint, bool isTrusted, DateTime now)
    {
        if (string.IsNullOrWhiteSpace(profileCode))
        {
            throw new InvalidOperationException("El código de perfil es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(fingerprint))
        {
            throw new InvalidOperationException("El fingerprint del dispositivo es obligatorio.");
        }

        return new UserSecurityAudit
        {
            UvaCode = Guid.NewGuid().ToString(),
            ProfileCode = profileCode,
            LastIpAddress = ip ?? "unknown",
            DeviceFingerprint = fingerprint,
            IsTrusted = isTrusted,
            LastCodeSentAt = now,
            CreatedAt = now
        };
    }

    public static UserSecurityAudit Rehydrate(
        string uvaCode,
        string profileCode,
        string lastIpAddress,
        string deviceFingerprint,
        DateTime? lastCodeSentAt,
        bool isTrusted,
        DateTime createdAt,
        DateTime? updatedAt)
    {
        return new UserSecurityAudit
        {
            UvaCode = uvaCode,
            ProfileCode = profileCode,
            LastIpAddress = lastIpAddress,
            DeviceFingerprint = deviceFingerprint,
            LastCodeSentAt = lastCodeSentAt,
            IsTrusted = isTrusted,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
    }

    public void MarkAsTrusted(DateTime now)
    {
        IsTrusted = true;
        UpdatedAt = now;
    }

    public void UpdateLastCodeSent(DateTime now)
    {
        LastCodeSentAt = now;
        UpdatedAt = now;
    }

    public void UpdateIpAddress(string ip, DateTime now)
    {
        LastIpAddress = ip ?? "unknown";
        UpdatedAt = now;
    }
}
