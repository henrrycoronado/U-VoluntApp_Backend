namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Contract;

using U_VoluntApp_Core.Src.Domain.Entities.Contract;
using U_VoluntApp_Core.Src.Domain.Utils.Configuration;

public interface IRoleRequestRepository
{
    Task<RoleRequest?> GetByCodeAsync(string uvaCode);

    Task<IEnumerable<RoleRequest>> GetAllAsync(RequestFilter filter);

    Task<IEnumerable<RoleRequest>> GetAllByRoleCodeAsync(string roleCode, RequestFilter filter);

    Task<IEnumerable<RoleRequest>> GetByProfileCodeAsync(string profileCode, RequestFilter filter);

    Task AddAsync(RoleRequest roleRequest);

    Task UpdateAsync(RoleRequest roleRequest);

    Task DeleteAsync(string uvaCode);
}
