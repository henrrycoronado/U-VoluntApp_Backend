namespace U_VoluntApp_Core.Src.Presentation.Extensions;

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public static class AuthConfigurationExtensions
{
    public static IServiceCollection AddAuthConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSecret = configuration["JWT_SECRET"]
            ?? throw new InvalidOperationException("Falta JWT_SECRET");
        var jwtIssuer = configuration["JWT_ISSUER"]
            ?? throw new InvalidOperationException("Falta JWT_ISSUER");
        var jwtAudience = configuration["JWT_AUDIENCE"]
            ?? throw new InvalidOperationException("Falta JWT_AUDIENCE");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSecret)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });

        return services;
    }
}
