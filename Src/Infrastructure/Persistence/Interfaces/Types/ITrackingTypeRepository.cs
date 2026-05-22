namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Types;

using U_VoluntApp_Backend.Src.Domain.Types;

public interface ITrackingTypeRepository
{
    Task<IEnumerable<TrackingType>> GetAllAsync();

    Task<TrackingType?> GetByCodeAsync(string uvaCode);

    Task<TrackingType?> GetByNameAsync(string name);

    Task AddAsync(TrackingType type);

    Task UpdateAsync(TrackingType type);

    Task DeleteAsync(string uvaCode);
}
