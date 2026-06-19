namespace U_VoluntApp_Backend.Src.Presentation.Extensions;

using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence;

public static class DatabaseConfigurationExtensions
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["DB_CONNECTION_STRING"]
            ?? throw new InvalidOperationException("Falta DB_CONNECTION_STRING");

        var s3AccessKey = configuration["S3_ACCESS_KEY"]
            ?? throw new InvalidOperationException("Falta S3_ACCESS_KEY");
        var s3SecretKey = configuration["S3_SECRET_KEY"]
            ?? throw new InvalidOperationException("Falta S3_SECRET_KEY");
        var s3EndpointUrl = configuration["S3_ENDPOINT_URL"]
            ?? throw new InvalidOperationException("Falta S3_ENDPOINT_URL");

        var s3Credentials = new BasicAWSCredentials(s3AccessKey, s3SecretKey);
        var s3Config = new AmazonS3Config
        {
            ServiceURL = s3EndpointUrl,
            ForcePathStyle = true,
            AuthenticationRegion = "auto",
            UseHttp = false
        };

        services.AddScoped<IAmazonS3>(_ => new AmazonS3Client(s3Credentials, s3Config));

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