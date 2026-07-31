namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Activity;

using U_VoluntApp_Core.Src.Domain.Entities.Activity;
using U_VoluntApp_Core.Src.Domain.Utils.Configuration;

public interface IActivityRepository
{
    Task<Activity?> GetByCodeAsync(string uvaCode);

    Task<IEnumerable<Activity>> GetByProgramCodeAsync(string programCode, RequestFilter filter);

    Task AddAsync(Activity activity);

    Task UpdateAsync(Activity activity);

    Task DeleteAsync(string uvaCode);
}
