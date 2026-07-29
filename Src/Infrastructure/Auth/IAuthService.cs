namespace U_VoluntApp_Core.Src.Infrastructure.Auth;

using U_VoluntApp_Core.Src.Application.DTOs;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);

    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);

    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);

    Task LogoutAsync(string profileCode, LogoutRequestDto request);
}
