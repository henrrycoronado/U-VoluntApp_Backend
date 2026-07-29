namespace U_VoluntApp_Core.Src.Application.DTOs;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class RegisterRequestDto
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email es inválido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener mínimo 8 caracteres")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es requerido")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres")]
    public string LastName { get; set; } = null!;

    [Phone(ErrorMessage = "El formato del teléfono es inválido")]
    public string? Phone { get; set; }

    public string? CareerCode { get; set; }
}

public class LoginRequestDto
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email es inválido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es requerida")]
    public string Password { get; set; } = null!;
}

public class AuthResponseDto
{
    public string? Token { get; set; }

    public DateTime? AccessTokenExpiresAtUtc { get; set; }

    public string? RefreshToken { get; set; }

    public string? UvaCode { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public List<string> Roles { get; set; } = new();

    public bool RequiresVerification { get; set; }
}

public class RefreshTokenRequestDto
{
    [Required(ErrorMessage = "El refresh token es requerido")]
    public string RefreshToken { get; set; } = null!;
}

public class LogoutRequestDto
{
    [Required(ErrorMessage = "El refresh token es requerido")]
    public string RefreshToken { get; set; } = null!;
}

public class VerifyEmailRequestDto
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email es inválido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "El código es requerido")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "El código debe tener 6 dígitos")]
    public string Code { get; set; } = null!;
}

public class VerifyDeviceRequestDto
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email es inválido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "El código es requerido")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "El código debe tener 6 dígitos")]
    public string Code { get; set; } = null!;

    [Required(ErrorMessage = "El fingerprint es requerido")]
    public string DeviceFingerprint { get; set; } = null!;
}

public class GoogleLoginRequestDto
{
    [Required(ErrorMessage = "El token de Google es requerido")]
    public string IdToken { get; set; } = null!;

    public string? Password { get; set; }
}

public class DeviceDto
{
    public string UvaCode { get; set; } = null!;

    public string LastIpAddress { get; set; } = null!;

    public string DeviceFingerprint { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? LastCodeSentAt { get; set; }

    public bool IsTrusted { get; set; }
}

public class RevokeDeviceRequestDto
{
    [Required(ErrorMessage = "El código es requerido")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "El código de verificación debe tener 6 dígitos")]
    public string Code { get; set; } = null!;
}

public class SendOtpRequestDto
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email es inválido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "El propósito es requerido")]
    public string Purpose { get; set; } = null!;
}
