namespace U_VoluntApp_Core.Src.Presentation.Extensions;

public static class CorsConfigurationExtensions
{
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            var allowedOrigins = (Environment.GetEnvironmentVariable("CORS_ALLOWED_ORIGINS") ?? "http://localhost:3000,http://localhost:5000")
                .Split(',')
                .Select(o => o.Trim())
                .ToArray();

            options.AddPolicy("AllowSpecificOrigins", policy =>
            {
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials()
                      .WithExposedHeaders("X-Response-Time-Ms");
            });
        });

        return services;
    }
}
