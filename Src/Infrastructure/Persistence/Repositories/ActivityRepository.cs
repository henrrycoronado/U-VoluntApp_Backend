namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Backend.Src.Domain.Entities.Activity;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Activity;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Mappers;

public class ActivityRepository : IActivityRepository
{
    private readonly AppDbContext _context;

    public ActivityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Activity?> GetByCodeAsync(string uvaCode)
    {
        var activity = await _context.Activities
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.UvaCode == uvaCode);

        return activity is null ? null : DomainPersistenceMapper.ToDomain(activity);
    }

    public async Task<IEnumerable<Activity>> GetByProgramCodeAsync(string programCode, RequestFilter filter)
    {
        var query = _context.Activities
            .AsNoTracking()
            .Where(a => a.ProgramCode == programCode);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(a => a.StateCode == filter.StateName);
        }

        var activities = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return activities.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task AddAsync(Activity activity)
    {
        var model = DomainPersistenceMapper.ToPersistence(activity);
        await _context.Activities.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Activity activity)
    {
        var model = DomainPersistenceMapper.ToPersistence(activity);
        _context.Activities.Update(model);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string uvaCode)
    {
        var activity = await _context.Activities.FirstOrDefaultAsync(a => a.UvaCode == uvaCode);
        if (activity != null)
        {
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
        }
    }
}
