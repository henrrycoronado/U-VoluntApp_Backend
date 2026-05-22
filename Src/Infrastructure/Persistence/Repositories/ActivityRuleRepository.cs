namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Backend.Src.Domain.Entities.Activity;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Activity;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Mappers;

public class ActivityRuleRepository : IActivityRuleRepository
{
    private readonly AppDbContext _context;

    public ActivityRuleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ActivityRule?> GetByActivityCodeAsync(string activityCode)
    {
        var rule = await _context.ActivityRules
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.ActivityCode == activityCode);

        return rule is null ? null : DomainPersistenceMapper.ToDomain(rule);
    }

    public async Task AddAsync(ActivityRule rule)
    {
        var model = DomainPersistenceMapper.ToPersistence(rule);
        await _context.ActivityRules.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ActivityRule rule)
    {
        var model = DomainPersistenceMapper.ToPersistence(rule);
        _context.ActivityRules.Update(model);
        await _context.SaveChangesAsync();
    }
}
