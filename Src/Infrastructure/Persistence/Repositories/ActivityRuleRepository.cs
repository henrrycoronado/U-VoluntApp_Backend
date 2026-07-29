namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Core.Src.Domain.Entities.Activity;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Activity;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Mappers;

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
        var existing = await _context.ActivityRules
            .FirstOrDefaultAsync(r => r.UvaCode == rule.UvaCode)
            ?? throw new InvalidOperationException("Regla de actividad no encontrada para actualizar");

        existing.RequiresEnrollment = rule.RequiresEnrollment;
        existing.EnrollmentDeadline = rule.EnrollmentDeadline;
        existing.RequiresApproval = rule.RequiresApproval;
        existing.TotalCapacity = rule.TotalCapacity;
        existing.CostAmount = rule.CostAmount;
        existing.CountsVolunteerHours = rule.CountsVolunteerHours;
        existing.PhotoUrl = rule.PhotoUrl;
        existing.UpdatedAt = rule.UpdatedAt;

        await _context.SaveChangesAsync();
    }
}
