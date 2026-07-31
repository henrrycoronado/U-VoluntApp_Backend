namespace U_VoluntApp_Core.Src.Infrastructure.Auth;

using System.Collections.Generic;
using System.Threading.Tasks;
using U_VoluntApp_Core.Src.Application.DTOs;

public interface IDeviceService
{
    Task<List<DeviceDto>> GetTrustedDevicesAsync(string profileCode);

    Task RegisterDeviceAsync(string profileCode, string ip, string fingerprint, bool isTrusted);

    Task<bool> IsDeviceTrustedAsync(string profileCode, string fingerprint);

    Task RevokeDeviceAsync(string profileCode, string deviceCode, string otpCode);
}
