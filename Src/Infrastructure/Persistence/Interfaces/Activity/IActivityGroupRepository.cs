namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Activity;

using U_VoluntApp_Backend.Src.Domain.Entities.Activity;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public interface IActivityGroupRepository
{
    Task<ActivityGroup?> GetByCodeAsync(string uvaCode);

    Task<IEnumerable<ActivityGroup>> GetByActivityCodeAsync(string activityCode, RequestFilter filter);

    Task AddAsync(ActivityGroup group);

    Task UpdateAsync(ActivityGroup group);

    Task DeleteAsync(string uvaCode);
}
