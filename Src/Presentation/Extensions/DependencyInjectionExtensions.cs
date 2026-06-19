namespace U_VoluntApp_Backend.Src.Presentation.Extensions;

using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Application.Services;
using U_VoluntApp_Backend.Src.Domain.Utils.Factories;
using U_VoluntApp_Backend.Src.Infrastructure.Auth;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Activity;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Contract;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Enrollment;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Profile;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Tracking;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.VolProgram;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Repositories;
using U_VoluntApp_Backend.Src.Infrastructure.Reports;
using U_VoluntApp_Backend.Src.Infrastructure.Storage;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IVolProgramRepository, VolProgramRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IActivityRuleRepository, ActivityRuleRepository>();
        services.AddScoped<IActivityGroupRepository, ActivityGroupRepository>();
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<ITrackingLogRepository, TrackingLogRepository>();
        services.AddScoped<IEvidenceRepository, EvidenceRepository>();
        services.AddScoped<IUserScholarshipRepository, UserScholarshipRepository>();
        services.AddScoped<IProgramCollaboratorRepository, ProgramCollaboratorRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IRoleRequestRepository, RoleRequestRepository>();

        // Infrastructure Services
        services.AddScoped<IAuthService, IdentityAuthService>();
        services.AddScoped<IStorageService, StorageService>();

        // Application Services
        services.AddScoped<IVolProgramService, VolProgramService>();
        services.AddScoped<IActivityService, ActivityService>();
        services.AddScoped<IEnrollmentService, EnrollmentService>();
        services.AddScoped<IActivityFactory, ActivityFactory>();
        services.AddScoped<ITrackingService, TrackingService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IUserScholarshipService, UserScholarshipService>();
        services.AddScoped<IProgramCollaboratorService, ProgramCollaboratorService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IReferenceCatalogService, ReferenceCatalogService>();
        services.AddScoped<IPdfReportService, ScholarshipPdfService>();
        services.AddScoped<IRoleRequestService, RoleRequestService>();

        return services;
    }
}
