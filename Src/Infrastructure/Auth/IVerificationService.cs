namespace U_VoluntApp_Core.Src.Infrastructure.Auth;

using System.Threading.Tasks;
using U_VoluntApp_Core.Src.Application.DTOs;

public interface IVerificationService
{
    Task SendOtpAsync(string email, string purpose);

    Task<bool> VerifyOtpAsync(string email, string purpose, string code);

    Task<AuthResponseDto> VerifyEmailAsync(VerifyEmailRequestDto request);

    Task<AuthResponseDto> VerifyDeviceAsync(VerifyDeviceRequestDto request);
}
