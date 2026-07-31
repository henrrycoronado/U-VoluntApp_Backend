namespace U_VoluntApp_Core.Src.Infrastructure.Auth;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using U_VoluntApp_Core.Src.Application.DTOs;
using U_VoluntApp_Core.Src.Domain.Utils.Enums;
using U_VoluntApp_Core.Src.Infrastructure.Email;
using U_VoluntApp_Core.Src.Infrastructure.Persistence;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Profile;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Auth;
using Profile = U_VoluntApp_Core.Src.Domain.Entities.Profile.Profile;

public class VerificationService : IVerificationService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IProfileRepository _profileRepository;
    private readonly IDeviceService _deviceService;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _context;

    public VerificationService(
        UserManager<IdentityUser> userManager,
        IEmailService emailService,
        IProfileRepository profileRepository,
        IDeviceService deviceService,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        AppDbContext context)
    {
        _userManager = userManager;
        _emailService = emailService;
        _profileRepository = profileRepository;
        _deviceService = deviceService;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    public async Task SendOtpAsync(string email, string purpose)
    {
        var identityUser = await _userManager.FindByEmailAsync(email)
            ?? throw new InvalidOperationException("Usuario no encontrado");

        var code = await _userManager.GenerateUserTokenAsync(identityUser, "Email", purpose);

        string subject;
        string body;

        switch (purpose)
        {
            case "EmailConfirmation":
                subject = "Verifica tu cuenta - U-VoluntApp";
                body = $"<p>¡Gracias por registrarte! Usa el siguiente código para activar tu cuenta:</p><h3>{code}</h3>";
                break;
            case "DeviceVerification":
                subject = "Verificación de dispositivo nuevo - U-VoluntApp";
                body = $"<p>Hemos detectado un inicio de sesión desde un dispositivo nuevo o no verificado. Usa este código para confirmar tu identidad:</p><h3>{code}</h3>";
                break;
            case "DeviceRevocation":
                subject = "Confirmación de seguridad - Eliminar dispositivo";
                body = $"<p>Estás intentando eliminar un dispositivo de confianza en U-VoluntApp. Usa este código para confirmar la acción:</p><h3>{code}</h3>";
                break;
            default:
                subject = "Código de seguridad - U-VoluntApp";
                body = $"<p>Usa el siguiente código de verificación para completar la acción solicitada:</p><h3>{code}</h3>";
                break;
        }

        await _emailService.SendEmailAsync(email, subject, body);
    }

    public async Task<bool> VerifyOtpAsync(string email, string purpose, string code)
    {
        var identityUser = await _userManager.FindByEmailAsync(email)
            ?? throw new InvalidOperationException("Usuario no encontrado");

        return await _userManager.VerifyUserTokenAsync(identityUser, "Email", purpose, code);
    }

    public async Task<AuthResponseDto> VerifyEmailAsync(VerifyEmailRequestDto request)
    {
        var identityUser = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new InvalidOperationException("Usuario no encontrado");

        var isValid = await VerifyOtpAsync(request.Email, "EmailConfirmation", request.Code);
        if (!isValid)
        {
            throw new InvalidOperationException("Código de verificación inválido o expirado");
        }

        identityUser.EmailConfirmed = true;
        await _userManager.UpdateAsync(identityUser);

        var profile = await _profileRepository.GetByEmailAsync(request.Email)
            ?? throw new InvalidOperationException("Perfil no encontrado");

        var nowUtc = DateTime.UtcNow;
        profile.ChangeState(ProfileState.Active.GetUvaCode(), nowUtc);
        await _profileRepository.UpdateAsync(profile);

        // Auto-trust the device they used to verify
        var fingerprint = _httpContextAccessor.HttpContext?.Request.Headers["X-Device-Fingerprint"].ToString();
        if (string.IsNullOrEmpty(fingerprint))
        {
            fingerprint = "web-browser";
        }

        var ip = GetRequestIp();
        await _deviceService.RegisterDeviceAsync(profile.UvaCode, ip, fingerprint, true);

        return await GenerateAuthResponseAsync(profile, identityUser);
    }

    public async Task<AuthResponseDto> VerifyDeviceAsync(VerifyDeviceRequestDto request)
    {
        var identityUser = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new InvalidOperationException("Usuario no encontrado");

        var isValid = await VerifyOtpAsync(request.Email, "DeviceVerification", request.Code);
        if (!isValid)
        {
            throw new InvalidOperationException("Código de verificación inválido o expirado");
        }

        var profile = await _profileRepository.GetByEmailAsync(request.Email)
            ?? throw new InvalidOperationException("Perfil no encontrado");

        var ip = GetRequestIp();
        await _deviceService.RegisterDeviceAsync(profile.UvaCode, ip, request.DeviceFingerprint, true);

        return await GenerateAuthResponseAsync(profile, identityUser);
    }

    private async Task<AuthResponseDto> GenerateAuthResponseAsync(Profile profile, IdentityUser identityUser)
    {
        var refreshToken = await CreateRefreshTokenAsync(identityUser.Id, profile.UvaCode);
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();

        return await BuildAuthResponseAsync(profile, identityUser, refreshToken);
    }

    private async Task<AuthResponseDto> BuildAuthResponseAsync(Profile profile, IdentityUser identityUser, RefreshToken refreshToken)
    {
        var roles = await _userManager.GetRolesAsync(identityUser);
        var accessTokenExpiry = DateTime.UtcNow.AddMinutes(GetAccessTokenMinutes());

        return new AuthResponseDto
        {
            Token = await GenerateJwtAsync(profile, identityUser),
            AccessTokenExpiresAtUtc = accessTokenExpiry,
            RefreshToken = refreshToken.PlainToken,
            UvaCode = profile.UvaCode,
            Email = profile.Email,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Roles = roles.ToList(),
        };
    }

    private async Task<RefreshToken> CreateRefreshTokenAsync(string identityUserId, string profileCode)
    {
        var plainToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var refreshDays = int.Parse(_configuration["JWT_REFRESH_DAYS"] ?? "15");

        return await Task.FromResult(new RefreshToken
        {
            UvaCode = Guid.NewGuid().ToString(),
            IdentityUserId = identityUserId,
            ProfileCode = profileCode,
            TokenHash = ComputeSha256Hex(plainToken),
            ExpiresAt = DateTime.UtcNow.AddDays(refreshDays),
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = GetRequestIp(),
            UserAgent = GetUserAgent(),
            PlainToken = plainToken,
        });
    }

    private int GetAccessTokenMinutes()
    {
        return int.Parse(_configuration["JWT_EXPIRY_MINUTES"] ?? "60");
    }

    private async Task<string> GenerateJwtAsync(Profile profile, IdentityUser identityUser)
    {
        var secret = _configuration["JWT_SECRET"]!;
        var issuer = _configuration["JWT_ISSUER"]!;
        var audience = _configuration["JWT_AUDIENCE"]!;
        var expiry = GetAccessTokenMinutes();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var roles = await _userManager.GetRolesAsync(identityUser);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, profile.UvaCode),
            new Claim(JwtRegisteredClaimNames.Email, profile.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("firstName", profile.FirstName),
            new Claim("lastName", profile.LastName),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiry),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string ComputeSha256Hex(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private string GetRequestIp()
    {
        return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
    }

    private string? GetUserAgent()
    {
        return _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();
    }
}
