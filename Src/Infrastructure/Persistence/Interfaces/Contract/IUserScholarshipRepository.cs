namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Contract;

using U_VoluntApp_Core.Src.Domain.Entities.Contract;
using U_VoluntApp_Core.Src.Domain.Utils.Configuration;

public interface IUserScholarshipRepository
{
    Task<UserScholarship?> GetByCodeAsync(string uvaCode);

    Task<IEnumerable<UserScholarship>> GetAllAsync(RequestFilter filter);

    Task<IEnumerable<UserScholarship>> GetByProfileCodeAsync(string profileCode, RequestFilter filter);

    Task<IEnumerable<UserScholarship>> GetByCareerCodeAsync(string careerCode, RequestFilter filter);

    Task<IEnumerable<UserScholarship>> GetByTypeCodeAsync(string typeCode, RequestFilter filter);

    Task<IEnumerable<UserScholarship>> GetByRequiredHoursAsync(int requiredHours, RequestFilter filter);

    Task AddAsync(UserScholarship scholarship);

    Task UpdateAsync(UserScholarship scholarship);

    Task DeleteAsync(string uvaCode);
}
