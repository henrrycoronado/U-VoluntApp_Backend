namespace U_VoluntApp_Backend.Src.Presentation.Extensions;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Supabase;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence;

public static class DatabaseConfigurationExtensions
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["DB_CONNECTION_STRING"]
            ?? throw new InvalidOperationException("Falta DB_CONNECTION_STRING");
        var storageUrl = configuration["STORAGE_URL"]
            ?? throw new InvalidOperationException("Falta STORAGE_URL");
        var storageKey = configuration["STORAGE_SERVICE_ROLE_KEY"]
            ?? throw new InvalidOperationException("Falta STORAGE_SERVICE_ROLE_KEY");

        services.AddScoped<Supabase.Client>(_ =>
            new Supabase.Client(storageUrl, storageKey, new SupabaseOptions
            {
                AutoRefreshToken = false,
                AutoConnectRealtime = false,
            }));

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddIdentityCore<IdentityUser>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>();

        return services;
    }
}
