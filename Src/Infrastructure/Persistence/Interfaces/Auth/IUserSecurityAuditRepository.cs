namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Auth;

using System.Collections.Generic;
using System.Threading.Tasks;
using U_VoluntApp_Core.Src.Domain.Entities.Auth;

public interface IUserSecurityAuditRepository
{
    Task<UserSecurityAudit?> GetByProfileAndFingerprintAsync(string profileCode, string fingerprint);

    Task<UserSecurityAudit?> GetByCodeAsync(string profileCode, string deviceCode);

    Task<List<UserSecurityAudit>> GetByProfileCodeAsync(string profileCode);

    Task AddAsync(UserSecurityAudit audit);

    Task UpdateAsync(UserSecurityAudit audit);

    Task DeleteAsync(string profileCode, string deviceCode);
}
