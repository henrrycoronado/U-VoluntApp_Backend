namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.States;

using U_VoluntApp_Backend.Src.Domain.States;

public interface ITrackingStateRepository
{
    Task<IEnumerable<TrackingState>> GetAllAsync();

    Task<TrackingState?> GetByCodeAsync(string uvaCode);

    Task<TrackingState?> GetByNameAsync(string name);

    Task UpdateAsync(TrackingState state);
}
