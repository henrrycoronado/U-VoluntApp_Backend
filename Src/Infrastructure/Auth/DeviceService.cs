namespace U_VoluntApp_Core.Src.Infrastructure.Auth;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using U_VoluntApp_Core.Src.Application.DTOs;
using U_VoluntApp_Core.Src.Domain.Entities.Auth;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Auth;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Profile;

public class DeviceService : IDeviceService
{
    private readonly IUserSecurityAuditRepository _securityAuditRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly UserManager<IdentityUser> _userManager;

    public DeviceService(
        IUserSecurityAuditRepository securityAuditRepository,
        IProfileRepository profileRepository,
        UserManager<IdentityUser> userManager)
    {
        _securityAuditRepository = securityAuditRepository;
        _profileRepository = profileRepository;
        _userManager = userManager;
    }

    public async Task<List<DeviceDto>> GetTrustedDevicesAsync(string profileCode)
    {
        var devices = await _securityAuditRepository.GetByProfileCodeAsync(profileCode);

        return devices.Select(d => new DeviceDto
        {
            UvaCode = d.UvaCode,
            LastIpAddress = d.LastIpAddress,
            DeviceFingerprint = d.DeviceFingerprint,
            CreatedAt = d.CreatedAt,
            LastCodeSentAt = d.LastCodeSentAt,
            IsTrusted = d.IsTrusted
        }).ToList();
    }

    public async Task RegisterDeviceAsync(string profileCode, string ip, string fingerprint, bool isTrusted)
    {
        var nowUtc = DateTime.UtcNow;
        var audit = await _securityAuditRepository.GetByProfileAndFingerprintAsync(profileCode, fingerprint);

        if (audit is null)
        {
            audit = UserSecurityAudit.Create(profileCode, ip, fingerprint, isTrusted, nowUtc);
            await _securityAuditRepository.AddAsync(audit);
        }
        else
        {
            if (isTrusted)
            {
                audit.MarkAsTrusted(nowUtc);
            }

            audit.UpdateIpAddress(ip, nowUtc);
            await _securityAuditRepository.UpdateAsync(audit);
        }
    }

    public async Task<bool> IsDeviceTrustedAsync(string profileCode, string fingerprint)
    {
        var audit = await _securityAuditRepository.GetByProfileAndFingerprintAsync(profileCode, fingerprint);
        return audit is not null && audit.IsTrusted;
    }

    public async Task RevokeDeviceAsync(string profileCode, string deviceCode, string otpCode)
    {
        var profile = await _profileRepository.GetByCodeAsync(profileCode)
            ?? throw new InvalidOperationException("Perfil no encontrado");

        var user = await _userManager.FindByEmailAsync(profile.Email)
            ?? throw new InvalidOperationException("Usuario no encontrado");

        var isValid = await _userManager.VerifyUserTokenAsync(user, "Email", "DeviceRevocation", otpCode);
        if (!isValid)
        {
            throw new InvalidOperationException("Código de verificación de revocación inválido o expirado");
        }

        await _securityAuditRepository.DeleteAsync(profileCode, deviceCode);
    }
}
