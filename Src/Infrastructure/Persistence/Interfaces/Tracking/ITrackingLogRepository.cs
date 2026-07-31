namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Tracking;

using U_VoluntApp_Core.Src.Domain.Entities.Tracking;
using U_VoluntApp_Core.Src.Domain.Utils.Configuration;

public interface ITrackingLogRepository
{
    Task<TrackingLog?> GetByCodeAsync(string uvaCode);

    Task<IEnumerable<TrackingLog>> GetByActivityCodeAsync(string activityCode, RequestFilter filter);

    Task<IEnumerable<TrackingLog>> GetByGroupActivityCodeAsync(string groupActivityCode, RequestFilter filter);

    Task<IEnumerable<TrackingLog>> GetByEnrollmentCodeAsync(string enrollmentCode, RequestFilter filter);

    Task<IEnumerable<TrackingLog>> GetByGroupEnrollmentCodeAsync(string groupEnrollmentCode, RequestFilter filter);

    Task<IEnumerable<TrackingLog>> GetByProfileCodeAsync(string profileCode, RequestFilter filter);

    Task AddAsync(TrackingLog log);

    Task UpdateAsync(TrackingLog log);

    Task DeleteAsync(string uvaCode);
}
