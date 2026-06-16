namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Profile;

using U_VoluntApp_Backend.Src.Domain.Entities.Profile;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public interface IScholarshipPerformanceRepository
{
    Task<ScholarshipPerformance?> GetScholarshipPerformanceByProfileCodeAsync(string profileCode);

    Task<IEnumerable<ScholarshipPerformance>> GetScholarshipPerformanceAsync(RequestFilter requestFilter);

    Task<IEnumerable<ScholarshipPerformance>> GetScholarshipPerformanceByTypeAsync(string scholarshipType, RequestFilter filter);

    Task<IEnumerable<ScholarshipPerformance>> GetScholarshipPerformanceByCompletionPercentageAsync(decimal minPercentage, decimal maxPercentage, RequestFilter filter);

    Task<IEnumerable<ScholarshipPerformance>> GetScholarshipPerformanceByCompletedHoursAsync(decimal minHours, decimal maxHours, RequestFilter filter);

    Task RefreshMaterializedViewsAsync();
}
