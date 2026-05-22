namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Backend.Src.Domain.Entities.Activity;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Activity;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Mappers;

public class ActivityGroupRepository : IActivityGroupRepository
{
    private readonly AppDbContext _context;

    public ActivityGroupRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ActivityGroup?> GetByCodeAsync(string uvaCode)
    {
        var group = await _context.ActivityGroups
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.UvaCode == uvaCode);

        return group is null ? null : DomainPersistenceMapper.ToDomain(group);
    }

    public async Task<IEnumerable<ActivityGroup>> GetByActivityCodeAsync(string activityCode, RequestFilter filter)
    {
        var query = _context.ActivityGroups
            .AsNoTracking()
            .Where(g => g.ActivityCode == activityCode);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(g => g.StateCode == filter.StateName);
        }

        var groups = await query
            .OrderBy(g => g.Name)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return groups.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task AddAsync(ActivityGroup group)
    {
        var model = DomainPersistenceMapper.ToPersistence(group);
        await _context.ActivityGroups.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ActivityGroup group)
    {
        var existing = await _context.ActivityGroups
            .FirstOrDefaultAsync(g => g.UvaCode == group.UvaCode)
            ?? throw new InvalidOperationException("Grupo de actividad no encontrado para actualizar");

        existing.Name = group.Name;
        existing.Details = group.Details;
        existing.TotalCapacity = group.TotalCapacity;
        existing.StartDate = group.StartDate;
        existing.EndDate = group.EndDate;
        existing.StateCode = group.StateCode;
        existing.UpdatedAt = group.UpdatedAt;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string uvaCode)
    {
        var group = await _context.ActivityGroups.FirstOrDefaultAsync(g => g.UvaCode == uvaCode);
        if (group != null)
        {
            _context.ActivityGroups.Remove(group);
            await _context.SaveChangesAsync();
        }
    }
}
