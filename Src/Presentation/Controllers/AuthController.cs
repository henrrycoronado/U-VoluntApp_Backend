namespace U_VoluntApp_Core.Src.Presentation.Controllers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U_VoluntApp_Core.Src.Application.DTOs;
using U_VoluntApp_Core.Src.Infrastructure.Auth;
using U_VoluntApp_Core.Src.Presentation.Helpers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IDeviceService _deviceService;
    private readonly IVerificationService _verificationService;

    public AuthController(
        IAuthService authService,
        IDeviceService deviceService,
        IVerificationService verificationService)
    {
        _authService = authService;
        _deviceService = deviceService;
        _verificationService = verificationService;
    }

    /// <summary>
    /// Registra un nuevo usuario en el sistema.
    /// </summary>
    /// <param name="request">El parametro request.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(
            [FromBody] RegisterRequestDto request)
    {
        var result = await _authService.RegisterAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Inicia sesión y genera un token JWT.
    /// </summary>
    /// <param name="request">El parametro request.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(
            [FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción Refresh para autenticación.
    /// </summary>
    /// <param name="request">El parametro request.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> Refresh(
            [FromBody] RefreshTokenRequestDto request)
    {
        var result = await _authService.RefreshTokenAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción Logout para autenticación.
    /// </summary>
    /// <param name="request">El parametro request.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        [FromBody] LogoutRequestDto request)
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        await _authService.LogoutAsync(profileCode, request);
        return NoContent();
    }

    /// <summary>
    /// Verifica el código de correo para activar la cuenta.
    /// </summary>
    /// <param name="request">El parametro request.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPost("verify-email")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponseDto>> VerifyEmail(
        [FromBody] VerifyEmailRequestDto request)
    {
        var result = await _verificationService.VerifyEmailAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Verifica el código de dispositivo nuevo para permitir el acceso.
    /// </summary>
    /// <param name="request">El parametro request.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPost("verify-device")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponseDto>> VerifyDevice(
        [FromBody] VerifyDeviceRequestDto request)
    {
        var result = await _verificationService.VerifyDeviceAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Inicia sesión o registra mediante Google OAuth.
    /// </summary>
    /// <param name="request">El parametro request.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPost("google-login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponseDto>> GoogleLogin(
        [FromBody] GoogleLoginRequestDto request)
    {
        var result = await _authService.GoogleLoginAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene la lista de dispositivos registrados.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Authorize]
    [HttpGet("security/devices")]
    [ProducesResponseType(typeof(List<DeviceDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<DeviceDto>>> GetDevices()
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        var result = await _deviceService.GetTrustedDevicesAsync(profileCode);
        return Ok(result);
    }

    /// <summary>
    /// Elimina o revoca un dispositivo de confianza.
    /// </summary>
    /// <param name="deviceCode">El parametro deviceCode.</param>
    /// <param name="request">El parametro request.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Authorize]
    [HttpDelete("security/devices/{deviceCode}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RevokeDevice(string deviceCode, [FromBody] RevokeDeviceRequestDto request)
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        await _deviceService.RevokeDeviceAsync(profileCode, deviceCode, request.Code);
        return NoContent();
    }

    /// <summary>
    /// Genera y envía un código OTP por correo electrónico para un propósito específico.
    /// </summary>
    /// <param name="request">El parametro request.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPost("verify/otp")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpRequestDto request)
    {
        await _verificationService.SendOtpAsync(request.Email, request.Purpose);
        return NoContent();
    }
}
