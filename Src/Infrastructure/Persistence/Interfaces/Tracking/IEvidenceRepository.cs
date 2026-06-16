namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Tracking;

using U_VoluntApp_Backend.Src.Domain.Entities.Tracking;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public interface IEvidenceRepository
{
    Task<Evidence?> GetByCodeAsync(string uvaCode);

    Task<IEnumerable<Evidence>> GetByTrackingLogCodeAsync(string trackingLogCode, RequestFilter filter);

    Task AddAsync(Evidence evidence);

    Task UpdateAsync(Evidence evidence);

    Task DeleteAsync(string uvaCode);
}
