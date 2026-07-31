namespace U_VoluntApp_Core.Src.Presentation.Extensions;

using U_VoluntApp_Core.Src.Application.Interfaces;
using U_VoluntApp_Core.Src.Application.Services;
using U_VoluntApp_Core.Src.Domain.Utils.Factories;
using U_VoluntApp_Core.Src.Infrastructure.Auth;
using U_VoluntApp_Core.Src.Infrastructure.Email;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Activity;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Auth;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Contract;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Enrollment;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Profile;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Tracking;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.VolProgram;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Repositories;
using U_VoluntApp_Core.Src.Infrastructure.Reports;
using U_VoluntApp_Core.Src.Infrastructure.Storage;

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
        services.AddScoped<IVolProgramCollaboratorRepository, VolProgramCollaboratorRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IRoleRequestRepository, RoleRequestRepository>();
        services.AddScoped<IUserSecurityAuditRepository, UserSecurityAuditRepository>();

        // Infrastructure Services
        services.AddScoped<IAuthService, IdentityAuthService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IVerificationService, VerificationService>();
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<IEmailService, SmtpEmailService>();

        // Application Services
        services.AddScoped<IVolProgramService, VolProgramService>();
        services.AddScoped<IActivityService, ActivityService>();
        services.AddScoped<IEnrollmentService, EnrollmentService>();
        services.AddScoped<IActivityFactory, ActivityFactory>();
        services.AddScoped<ITrackingService, TrackingService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IUserScholarshipService, UserScholarshipService>();
        services.AddScoped<IVolProgramCollaboratorService, VolProgramCollaboratorService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IReferenceCatalogService, ReferenceCatalogService>();
        services.AddScoped<IPdfReportService, ScholarshipPdfService>();
        services.AddScoped<IRoleRequestService, RoleRequestService>();

        return services;
    }
}
