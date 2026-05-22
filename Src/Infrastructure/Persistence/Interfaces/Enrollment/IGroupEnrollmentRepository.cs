namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Enrollment;

using U_VoluntApp_Backend.Src.Domain.Entities.Enrollment;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public interface IGroupEnrollmentRepository
{
    Task<GroupEnrollment?> GetByCodeAsync(string uvaCode);

    Task<IEnumerable<GroupEnrollment>> GetByActivityGroupCodeAsync(string activityGroupCode, RequestFilter filter);

    Task<IEnumerable<GroupEnrollment>> GetByGroupActivityCodeAsync(string groupActivityCode, RequestFilter filter);

    Task<IEnumerable<GroupEnrollment>> GetByEnrollmentCodeAsync(string enrollmentCode, RequestFilter filter);

    Task<IEnumerable<GroupEnrollment>> GetByProfileCodeAsync(string profileCode, RequestFilter filter);

    Task AddAsync(GroupEnrollment groupEnrollment);

    Task UpdateAsync(GroupEnrollment groupEnrollment);

    Task DeleteAsync(string uvaCode);
}
