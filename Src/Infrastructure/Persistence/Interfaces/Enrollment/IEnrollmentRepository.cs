namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Enrollment;

using U_VoluntApp_Core.Src.Domain.Entities.Enrollment;
using U_VoluntApp_Core.Src.Domain.Utils.Configuration;

public interface IEnrollmentRepository
{
    Task<Enrollment?> GetByCodeAsync(string uvaCode);

    Task<IEnumerable<Enrollment>> GetByActivityCodeAsync(string activityCode, RequestFilter filter);

    Task<IEnumerable<Enrollment>> GetByProfileCodeAsync(string profileCode, RequestFilter filter);

    Task AddAsync(Enrollment enrollment);

    Task UpdateAsync(Enrollment enrollment);
}
