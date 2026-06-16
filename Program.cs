using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using Supabase;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Application.Services;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;
using U_VoluntApp_Backend.Src.Domain.Utils.Factories;
using U_VoluntApp_Backend.Src.Infrastructure.Auth;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Activity;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Contract;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Enrollment;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Profile;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Tracking;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.VolProgram;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Repositories;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Seeders;
using U_VoluntApp_Backend.Src.Infrastructure.Reports;
using U_VoluntApp_Backend.Src.Infrastructure.Storage;
using U_VoluntApp_Backend.Src.Presentation.Middleware;

QuestPDF.Settings.License = LicenseType.Community;

EnvLoader.Load();

var builder = WebApplication.CreateBuilder(args);

var jwtSecret = builder.Configuration["JWT_SECRET"]
    ?? throw new InvalidOperationException("Falta JWT_SECRET");
var jwtIssuer = builder.Configuration["JWT_ISSUER"]
    ?? throw new InvalidOperationException("Falta JWT_ISSUER");
var jwtAudience = builder.Configuration["JWT_AUDIENCE"]
    ?? throw new InvalidOperationException("Falta JWT_AUDIENCE");
var connectionString = builder.Configuration["DB_CONNECTION_STRING"]
    ?? throw new InvalidOperationException("Falta DB_CONNECTION_STRING");
var storageUrl = builder.Configuration["STORAGE_URL"]
    ?? throw new InvalidOperationException("Falta STORAGE_URL");
var storageKey = builder.Configuration["STORAGE_SERVICE_ROLE_KEY"]
    ?? throw new InvalidOperationException("Falta STORAGE_SERVICE_ROLE_KEY");

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            var message = "Error de validación: " + string.Join(" | ", errors);

            if (context.HttpContext.Request.Path.Value?.Contains("/auth/", StringComparison.OrdinalIgnoreCase) == true)
            {
                message = "Credenciales o datos de acceso inválidos.";
            }

            return new BadRequestObjectResult(new
            {
                error = message,
                code = StatusCodes.Status400BadRequest,
            });
        };
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IVolProgramRepository, VolProgramRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IActivityRuleRepository, ActivityRuleRepository>();
builder.Services.AddScoped<IActivityGroupRepository, ActivityGroupRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<ITrackingLogRepository, TrackingLogRepository>();
builder.Services.AddScoped<IEvidenceRepository, EvidenceRepository>();
builder.Services.AddScoped<IUserScholarshipRepository, UserScholarshipRepository>();
builder.Services.AddScoped<IProgramCollaboratorRepository, ProgramCollaboratorRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IRoleRequestRepository, RoleRequestRepository>();

builder.Services.AddScoped<IAuthService, IdentityAuthService>();
builder.Services.AddScoped<IStorageService, StorageService>();

builder.Services.AddScoped<IVolProgramService, VolProgramService>();
builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IActivityFactory, ActivityFactory>();
builder.Services.AddScoped<ITrackingService, TrackingService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IUserScholarshipService, UserScholarshipService>();
builder.Services.AddScoped<IProgramCollaboratorService, ProgramCollaboratorService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IReferenceCatalogService, ReferenceCatalogService>();
builder.Services.AddScoped<IPdfReportService, ScholarshipPdfService>();
builder.Services.AddScoped<IRoleRequestService, RoleRequestService>();

builder.Services.AddScoped<Supabase.Client>(_ =>
    new Supabase.Client(storageUrl, storageKey, new SupabaseOptions
    {
        AutoRefreshToken = false,
        AutoConnectRealtime = false,
    }));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentityCore<IdentityUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "UVoluntapp API",
        Version = "v1",
        Description = "API de gestión de voluntariado universitario",
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Ejemplo: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
            },
            Array.Empty<string>()
        },
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

builder.Services.AddCors(options =>
{
    var allowedOrigins = (Environment.GetEnvironmentVariable("CORS_ALLOWED_ORIGINS") ?? "http://localhost:3000,http://localhost:5000")
        .Split(',')
        .Select(o => o.Trim())
        .ToArray();

    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()

              // AllowCredentials()
              .WithExposedHeaders("X-Response-Time-Ms");
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var profileRepo = scope.ServiceProvider.GetRequiredService<IProfileRepository>();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

    await AuthSeeder.SeedRolesAndSuperUserAsync(roleManager, userManager, profileRepo, config);
    await DataSeeder.SeedInitialDataAsync(db, config);
}

var showSwagger = builder.Configuration.GetValue<bool>("SHOW_SWAGGER", builder.Environment.IsDevelopment());
if (showSwagger || app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "UVoluntapp API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseMiddleware<RequestValidationMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

var enableHttpsRedirection = builder.Configuration.GetValue("ENABLE_HTTPS_REDIRECTION", builder.Environment.IsDevelopment());
if (enableHttpsRedirection)
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
