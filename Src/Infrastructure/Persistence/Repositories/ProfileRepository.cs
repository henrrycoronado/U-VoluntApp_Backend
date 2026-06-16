namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Backend.Src.Domain.Entities.Profile;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Profile;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Mappers;

public class ProfileRepository : IProfileRepository
{
    private readonly AppDbContext _context;

    public ProfileRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Profile?> GetByCodeAsync(string uvaCode)
    {
        var profile = await _context.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UvaCode == uvaCode);

        return profile is null ? null : DomainPersistenceMapper.ToDomain(profile);
    }

    public async Task<Profile?> GetByIdentityUserIdAsync(string identityUserId)
    {
        var profile = await _context.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.IdentityUserId == identityUserId);

        return profile is null ? null : DomainPersistenceMapper.ToDomain(profile);
    }

    public async Task<Profile?> GetByEmailAsync(string email)
    {
        var profile = await _context.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Email == email);

        return profile is null ? null : DomainPersistenceMapper.ToDomain(profile);
    }

    public async Task<bool> ExistsByEmailAsync(string email) =>
        await _context.Profiles.AnyAsync(p => p.Email == email);

    public async Task AddAsync(Profile profile)
    {
        var model = DomainPersistenceMapper.ToPersistence(profile);
        await _context.Profiles.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Profile profile)
    {
        var model = DomainPersistenceMapper.ToPersistence(profile);
        _context.Profiles.Update(model);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string uvaCode)
    {
        var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UvaCode == uvaCode);
        if (profile != null)
        {
            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
        }
    }
}
