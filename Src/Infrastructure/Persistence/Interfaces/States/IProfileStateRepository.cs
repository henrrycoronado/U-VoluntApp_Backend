namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.States;

using U_VoluntApp_Backend.Src.Domain.States;

public interface IProfileStateRepository
{
    Task<IEnumerable<ProfileState>> GetAllAsync();

    Task<ProfileState?> GetByCodeAsync(string uvaCode);

    Task<ProfileState?> GetByNameAsync(string name);

    Task UpdateAsync(ProfileState state);
}
