namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.States;

using U_VoluntApp_Backend.Src.Domain.States;

public interface IRoleRequestStateRepository
{
    Task<IEnumerable<RoleRequestState>> GetAllAsync();

    Task<RoleRequestState?> GetByCodeAsync(string uvaCode);

    Task<RoleRequestState?> GetByNameAsync(string name);

    Task UpdateAsync(RoleRequestState state);
}
