namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Profile;

using U_VoluntApp_Core.Src.Domain.Entities.Profile;

public interface IProfileRepository
{
    Task<Profile?> GetByCodeAsync(string uvaCode);

    Task<Profile?> GetByIdentityUserIdAsync(string identityUserId);

    Task<Profile?> GetByEmailAsync(string email);

    Task<bool> ExistsByEmailAsync(string email);

    Task AddAsync(Profile profile);

    Task UpdateAsync(Profile profile);

    Task DeleteAsync(string uvaCode);
}
