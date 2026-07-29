namespace U_VoluntApp_Core.Src.Infrastructure.Auth;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using U_VoluntApp_Core.Src.Application.DTOs;
using U_VoluntApp_Core.Src.Domain.Entities.Profile;
using U_VoluntApp_Core.Src.Domain.Utils.Constants;
using U_VoluntApp_Core.Src.Domain.Utils.Enums;
using U_VoluntApp_Core.Src.Infrastructure.Persistence;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Contract;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Profile;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Auth;

public class IdentityAuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IProfileRepository _profileRepository;
    private readonly IConfiguration _configuration;
    private readonly IRoleRequestRepository _roleRequestRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _context;
    private readonly IDeviceService _deviceService;
    private readonly IVerificationService _verificationService;

    public IdentityAuthService(
        UserManager<IdentityUser> userManager,
        IProfileRepository profileRepository,
        IConfiguration configuration,
        IRoleRequestRepository roleRequestRepository,
        IHttpContextAccessor httpContextAccessor,
        AppDbContext context,
        IDeviceService deviceService,
        IVerificationService verificationService)
    {
        _userManager = userManager;
        _profileRepository = profileRepository;
        _configuration = configuration;
        _roleRequestRepository = roleRequestRepository;
        _httpContextAccessor = httpContextAccessor;
        _context = context;
        _deviceService = deviceService;
        _verificationService = verificationService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        ValidateInstitutionalEmail(request.Email);

        var userId = Guid.NewGuid().ToString();

        var identityUser = new IdentityUser
        {
            Id = userId,
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = false,
            NormalizedUserName = request.Email.ToUpperInvariant(),
            NormalizedEmail = request.Email.ToUpperInvariant(),
        };

        var identityResult = await _userManager.CreateAsync(identityUser, request.Password);

        if (!identityResult.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ", identityResult.Errors.Select(e => e.Description)));
        }

        var addRoleResult = await _userManager.AddToRoleAsync(identityUser, RoleConstants.VolunteerRole);
        if (!addRoleResult.Succeeded)
        {
            await _userManager.DeleteAsync(identityUser);
            throw new InvalidOperationException(
                string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
        }

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var nowUtc = DateTime.UtcNow;
                var profile = Profile.Create(
                    userId,
                    request.Email,
                    request.FirstName,
                    request.LastName,
                    ProfileState.Inactive.GetUvaCode(),
                    nowUtc);

                profile.ApplyUpdate(
                    request.FirstName,
                    request.LastName,
                    request.Phone,
                    "Dirección no registrada",
                    request.CareerCode ?? CareerType.None.GetUvaCode(),
                    0.00m,
                    nowUtc);

                await _profileRepository.AddAsync(profile);

                await transaction.CommitAsync();

                await _verificationService.SendOtpAsync(request.Email, "EmailConfirmation");

                return new AuthResponseDto
                {
                    Email = request.Email,
                    RequiresVerification = true
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                await _userManager.DeleteAsync(identityUser);
                throw;
            }
        }
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var identityUser = await _userManager.FindByEmailAsync(request.Email);

        if (identityUser == null || !await _userManager.CheckPasswordAsync(identityUser, request.Password))
        {
            throw new InvalidOperationException("Credenciales inválidas");
        }

        if (!identityUser.EmailConfirmed)
        {
            await _verificationService.SendOtpAsync(identityUser.Email!, "EmailConfirmation");
            throw new InvalidOperationException("El correo no ha sido verificado. Se ha enviado un nuevo código de activación.");
        }

        var profile = await _profileRepository.GetByEmailAsync(request.Email);

        if (profile is null)
        {
            profile = Profile.Create(
                identityUser.Id,
                request.Email,
                "Usuario",
                "Nuevo",
                ProfileState.Active.GetUvaCode(),
                DateTime.UtcNow);

            await _profileRepository.AddAsync(profile);
        }

        var fingerprint = _httpContextAccessor.HttpContext?.Request.Headers["X-Device-Fingerprint"].ToString();
        if (string.IsNullOrEmpty(fingerprint))
        {
            fingerprint = "web-browser";
        }

        var ip = GetRequestIp();

        var isTrusted = await _deviceService.IsDeviceTrustedAsync(profile.UvaCode, fingerprint);

        if (!isTrusted)
        {
            await _deviceService.RegisterDeviceAsync(profile.UvaCode, ip, fingerprint, false);
            await _verificationService.SendOtpAsync(identityUser.Email!, "DeviceVerification");

            return new AuthResponseDto
            {
                Email = request.Email,
                RequiresVerification = true
            };
        }

        await _deviceService.RegisterDeviceAsync(profile.UvaCode, ip, fingerprint, true);

        return await GenerateAuthResponseAsync(profile, identityUser);
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var hash = ComputeSha256Hex(request.RefreshToken);
        var tokenModel = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == hash);

        if (tokenModel is null || tokenModel.ExpiresAt <= DateTime.UtcNow)
        {
            throw new InvalidOperationException("Refresh token inválido o expirado");
        }

        var identityUser = await _userManager.FindByIdAsync(tokenModel.IdentityUserId)
            ?? throw new InvalidOperationException("Usuario no encontrado");

        var profile = await _profileRepository.GetByCodeAsync(tokenModel.ProfileCode)
            ?? throw new InvalidOperationException("Perfil no encontrado");

        _context.RefreshTokens.Remove(tokenModel);

        var newRefreshToken = await CreateRefreshTokenAsync(identityUser.Id, profile.UvaCode);
        await _context.RefreshTokens.AddAsync(newRefreshToken);
        await _context.SaveChangesAsync();

        return await BuildAuthResponseAsync(profile, identityUser, newRefreshToken);
    }

    public async Task LogoutAsync(string profileCode, LogoutRequestDto request)
    {
        var hash = ComputeSha256Hex(request.RefreshToken);
        var tokenModel = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.ProfileCode == profileCode && t.TokenHash == hash);

        if (tokenModel != null)
        {
            _context.RefreshTokens.Remove(tokenModel);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<AuthResponseDto> GoogleLoginAsync(GoogleLoginRequestDto request)
    {
        GoogleJsonWebSignature.Payload payload;
        try
        {
            var allowedClientIds = _configuration["GOOGLE_CLIENT_IDS"]?.Split(',') ?? Array.Empty<string>();
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = allowedClientIds
            };
            payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Token de Google inválido o expirado", ex);
        }

        var email = payload.Email;
        ValidateInstitutionalEmail(email);

        var desktopClientId = _configuration["GOOGLE_CLIENT_ID_DESKTOP"];
        var isDesktop = (payload.Audience as string) == desktopClientId;

        if (isDesktop && string.IsNullOrWhiteSpace(request.Password))
        {
            throw new InvalidOperationException("La contraseña es requerida para la verificación en la aplicación de escritorio.");
        }

        var identityUser = await _userManager.FindByEmailAsync(email);
        var nowUtc = DateTime.UtcNow;

        if (identityUser is null)
        {
            var userId = Guid.NewGuid().ToString();
            identityUser = new IdentityUser
            {
                Id = userId,
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                NormalizedUserName = email.ToUpperInvariant(),
                NormalizedEmail = email.ToUpperInvariant(),
            };

            IdentityResult identityResult;
            if (isDesktop)
            {
                identityResult = await _userManager.CreateAsync(identityUser, request.Password!);
            }
            else
            {
                identityResult = await _userManager.CreateAsync(identityUser);
            }

            if (!identityResult.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", identityResult.Errors.Select(e => e.Description)));
            }

            var addRoleResult = await _userManager.AddToRoleAsync(identityUser, RoleConstants.VolunteerRole);
            if (!addRoleResult.Succeeded)
            {
                await _userManager.DeleteAsync(identityUser);
                throw new InvalidOperationException(string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
            }

            var profile = Profile.Create(
                userId,
                email,
                payload.GivenName ?? "Usuario",
                payload.FamilyName ?? "Google",
                ProfileState.Active.GetUvaCode(),
                nowUtc);

            await _profileRepository.AddAsync(profile);

            var fingerprint = _httpContextAccessor.HttpContext?.Request.Headers["X-Device-Fingerprint"].ToString() ?? "google-oauth-device";
            var ip = GetRequestIp();

            await _deviceService.RegisterDeviceAsync(profile.UvaCode, ip, fingerprint, true);

            return await GenerateAuthResponseAsync(profile, identityUser);
        }
        else
        {
            if (isDesktop)
            {
                if (!await _userManager.CheckPasswordAsync(identityUser, request.Password!))
                {
                    throw new InvalidOperationException("Credenciales locales inválidas");
                }
            }

            var logins = await _userManager.GetLoginsAsync(identityUser);
            if (logins.All(l => l.LoginProvider != "Google"))
            {
                var addLoginResult = await _userManager.AddLoginAsync(identityUser, new UserLoginInfo("Google", payload.Subject, "Google"));
                if (!addLoginResult.Succeeded)
                {
                    throw new InvalidOperationException("No se pudo vincular la cuenta de Google");
                }
            }

            var profile = await _profileRepository.GetByEmailAsync(email);
            if (profile is null)
            {
                profile = Profile.Create(
                    identityUser.Id,
                    email,
                    payload.GivenName ?? "Usuario",
                    payload.FamilyName ?? "Google",
                    ProfileState.Active.GetUvaCode(),
                    nowUtc);
                await _profileRepository.AddAsync(profile);
            }
            else if (profile.StateCode == ProfileState.Inactive.GetUvaCode())
            {
                profile.ChangeState(ProfileState.Active.GetUvaCode(), nowUtc);
                await _profileRepository.UpdateAsync(profile);
                identityUser.EmailConfirmed = true;
                await _userManager.UpdateAsync(identityUser);
            }

            var fingerprint = _httpContextAccessor.HttpContext?.Request.Headers["X-Device-Fingerprint"].ToString() ?? "google-oauth-device";
            var ip = GetRequestIp();

            await _deviceService.RegisterDeviceAsync(profile.UvaCode, ip, fingerprint, true);

            return await GenerateAuthResponseAsync(profile, identityUser);
        }
    }

    private void ValidateInstitutionalEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new InvalidOperationException("El correo es obligatorio.");
        }

        var match = Regex.Match(email, @"@([\w\.\-]+)$");
        if (!match.Success)
        {
            throw new InvalidOperationException("El correo ingresado no es válido.");
        }

        var domain = match.Groups[1].Value.ToLowerInvariant();
        if (domain != "autonoma.cl" && domain != "uautonoma.cl")
        {
            throw new InvalidOperationException("Solo se permiten correos institucionales de la Universidad Autónoma.");
        }
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
