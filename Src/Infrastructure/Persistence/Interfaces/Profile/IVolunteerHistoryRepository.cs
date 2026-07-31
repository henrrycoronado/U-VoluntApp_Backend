namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Profile;

using U_VoluntApp_Core.Src.Domain.Entities.Profile;
using U_VoluntApp_Core.Src.Domain.Utils.Configuration;

public interface IVolunteerHistoryRepository
{
    Task<VolunteerHistory?> GetVolunteerHistoryByProfileCodeAsync(string profileCode);

    Task<IEnumerable<VolunteerHistory>> GetVolunteerHistoryAsync(RequestFilter filter);

    Task<IEnumerable<VolunteerHistory>> GetVolunteerHistoryByCareerAsync(string careerName, RequestFilter filter);

    Task<IEnumerable<VolunteerHistory>> GetVolunteerHistoryByValidatedHoursAsync(decimal minHours, decimal maxHours, RequestFilter filter);

    Task RefreshMaterializedViewsAsync();
}
