namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Types;

using U_VoluntApp_Backend.Src.Domain.Types;

public interface IActivityTypeRepository
{
    Task<IEnumerable<ActivityType>> GetAllAsync();

    Task<ActivityType?> GetByCodeAsync(string uvaCode);

    Task<ActivityType?> GetByNameAsync(string name);

    Task AddAsync(ActivityType type);

    Task UpdateAsync(ActivityType type);

    Task DeleteAsync(string uvaCode);
}
