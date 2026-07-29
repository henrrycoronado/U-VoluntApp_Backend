namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Profile;

using U_VoluntApp_Core.Src.Domain.Entities.Profile;
using U_VoluntApp_Core.Src.Domain.Utils.Configuration;

public interface IProgramAnalyticsRepository
{
    Task<ProgramAnalytics?> GetProgramAnalyticsByCodeAsync(string programCode);

    Task<IEnumerable<ProgramAnalytics>> GetProgramAnalyticsAsync(RequestFilter filter);

    Task RefreshMaterializedViewsAsync();
}
