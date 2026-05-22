namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.States;

using U_VoluntApp_Backend.Src.Domain.States;

public interface IActivityStateRepository
{
    Task<IEnumerable<ActivityState>> GetAllAsync();

    Task<ActivityState?> GetByCodeAsync(string uvaCode);

    Task<ActivityState?> GetByNameAsync(string name);

    Task UpdateAsync(ActivityState state);
}
