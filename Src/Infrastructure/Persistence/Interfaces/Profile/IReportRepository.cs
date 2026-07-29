namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Profile;

using U_VoluntApp_Core.Src.Domain.Entities.Profile;

public interface IReportRepository
{
    Task<IEnumerable<ScholarshipPerformance>> GetScholarshipPerformanceAsync();

    Task<IEnumerable<ScholarshipPerformance>> GetScholarshipPerformanceByTypeAsync(string scholarshipType);

    Task<IEnumerable<ProgramAnalytics>> GetProgramAnalyticsAsync();

    Task<ProgramAnalytics?> GetProgramAnalyticsByCodeAsync(string programCode);

    Task<IEnumerable<ActivityAnalytics>> GetActivityAnalyticsAsync();

    Task<IEnumerable<ActivityAnalytics>> GetActivityAnalyticsByProgramCodeAsync(string programCode);

    Task<IEnumerable<VolunteerHistory>> GetVolunteerHistoryAsync();

    Task<VolunteerHistory?> GetVolunteerHistoryByProfileCodeAsync(string profileCode);

    Task<VolunteerHistory?> GetLiveVolunteerHistoryByProfileCodeAsync(string profileCode);

    Task<HomeSummary?> GetLiveHomeSummaryByProfileCodeAsync(string profileCode, int year, int month);

    Task RefreshMaterializedViewsAsync();
}
