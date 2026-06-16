namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Profile;

using U_VoluntApp_Backend.Src.Domain.Entities.Profile;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public interface IActivityAnalyticsRepository
{
    Task<ActivityAnalytics?> GetActivityAnalyticsByCodeAsync(string activityCode);

    Task<IEnumerable<ActivityAnalytics>> GetActivityAnalyticsAsync(RequestFilter filter);

    Task<IEnumerable<ActivityAnalytics>> GetActivityAnalyticsByProgramCodeAsync(string programCode, RequestFilter filter);

    Task RefreshMaterializedViewsAsync();
}
