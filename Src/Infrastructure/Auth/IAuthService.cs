namespace U_VoluntApp_Backend.Src.Infrastructure.Auth;

using U_VoluntApp_Backend.Src.Application.DTOs;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);

    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);

    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);

    Task LogoutAsync(string profileCode, LogoutRequestDto request);
}
