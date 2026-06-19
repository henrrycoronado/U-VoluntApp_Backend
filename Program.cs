using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
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
using U_VoluntApp_Backend.Src.Presentation.Extensions;
using U_VoluntApp_Backend.Src.Presentation.Middleware;

QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

EnvLoader.Load();

builder.Host.ConfigureSerilog();

builder.Services.AddControllers()
    .AddCustomModelValidation();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddAppServices();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddAuthConfiguration(builder.Configuration);
builder.Services.AddCorsConfiguration();

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
