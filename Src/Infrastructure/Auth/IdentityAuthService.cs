namespace U_VoluntApp_Backend.Src.Infrastructure.Auth;

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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Entities.Profile;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;
using U_VoluntApp_Backend.Src.Domain.Utils.Enums;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Contract;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Profile;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Auth;

public class IdentityAuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IProfileRepository _profileRepository;
    private readonly IConfiguration _configuration;
    private readonly IRoleRequestRepository _roleRequestRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _context;

    public IdentityAuthService(
        UserManager<IdentityUser> userManager,
        IProfileRepository profileRepository,
        IConfiguration configuration,
        IRoleRequestRepository roleRequestRepository,
        IHttpContextAccessor httpContextAccessor,
        AppDbContext context)
    {
        _userManager = userManager;
        _profileRepository = profileRepository;
        _configuration = configuration;
        _roleRequestRepository = roleRequestRepository;
        _httpContextAccessor = httpContextAccessor;
        _context = context;
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
                    ProfileState.Active.GetUvaCode(),
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
                return await GenerateAuthResponseAsync(profile, identityUser);
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

        return await GenerateAuthResponseAsync(profile, identityUser);
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var refreshTokenHash = ComputeSha256Hex(request.RefreshToken);
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == refreshTokenHash);

        if (refreshToken is null)
        {
            throw new InvalidOperationException("Refresh token inválido");
        }

        if (refreshToken.RevokedAt.HasValue)
        {
            throw new InvalidOperationException("Refresh token revocado");
        }

        if (refreshToken.ExpiresAt <= DateTime.UtcNow)
        {
            throw new InvalidOperationException("Refresh token expirado");
        }

        var identityUser = await _userManager.FindByIdAsync(refreshToken.IdentityUserId)
            ?? throw new InvalidOperationException("Usuario no encontrado");

        var profile = await _profileRepository.GetByIdentityUserIdAsync(identityUser.Id)
            ?? throw new InvalidOperationException("Perfil no encontrado");

        var rotatedToken = await CreateRefreshTokenAsync(identityUser.Id, profile.UvaCode);

        refreshToken.RevokedAt = DateTime.UtcNow;
        refreshToken.RevokedByIp = GetRequestIp();
        refreshToken.ReasonRevoked = "Rotated";
        refreshToken.ReplacedByTokenHash = rotatedToken.TokenHash;

        _context.RefreshTokens.Update(refreshToken);
        await _context.RefreshTokens.AddAsync(rotatedToken);
        await _context.SaveChangesAsync();

        return await BuildAuthResponseAsync(profile, identityUser, rotatedToken);
    }

    public async Task LogoutAsync(string profileCode, LogoutRequestDto request)
    {
        var refreshTokenHash = ComputeSha256Hex(request.RefreshToken);
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == refreshTokenHash && t.ProfileCode == profileCode);

        if (refreshToken is null)
        {
            return;
        }

        if (!refreshToken.RevokedAt.HasValue)
        {
            refreshToken.RevokedAt = DateTime.UtcNow;
            refreshToken.RevokedByIp = GetRequestIp();
            refreshToken.ReasonRevoked = "User logout";

            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();
        }
    }

    private static string ComputeSha256Hex(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLowerInvariant();
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

    private string GetRequestIp()
    {
        return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
    }

    private string? GetUserAgent()
    {
        return _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();
    }

    private void ValidateInstitutionalEmail(string email)
    {
        const string allowedDomain = "ucb.edu.bo";
        var atPosition = email.IndexOf('@');
        if (atPosition < 0 || atPosition == email.Length - 1)
        {
            throw new InvalidOperationException("Formato de correo inválido");
        }

        var emailDomain = email[(atPosition + 1)..].ToLowerInvariant();

        if (!emailDomain.Equals(allowedDomain, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Solo se permiten correos del dominio @{allowedDomain}. Correo proporcionado: {email}");
        }
    }
}
