namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Profile;

using U_VoluntApp_Backend.Src.Domain.Entities.Profile;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public interface IProgramAnalyticsRepository
{
    Task<ProgramAnalytics?> GetProgramAnalyticsByCodeAsync(string programCode);

    Task<IEnumerable<ProgramAnalytics>> GetProgramAnalyticsAsync(RequestFilter filter);

    Task RefreshMaterializedViewsAsync();
}
