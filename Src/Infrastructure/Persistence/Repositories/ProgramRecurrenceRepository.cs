namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Core.Src.Domain.Entities.VolProgram;
using U_VoluntApp_Core.Src.Domain.Utils.Configuration;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.VolProgram;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Mappers;

public class ProgramRecurrenceRepository : IProgramRecurrenceRepository
{
    private readonly AppDbContext _context;

    public ProgramRecurrenceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ActivityRecurrencePattern?> GetByCodeAsync(string uvaCode)
    {
        var pattern = await _context.ActivityRecurrencePatterns
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UvaCode == uvaCode);

        return pattern is null ? null : DomainPersistenceMapper.ToDomain(pattern);
    }

    public async Task<IEnumerable<ActivityRecurrencePattern>> GetByProgramCodeAsync(string programCode, RequestFilter filter)
    {
        var query = _context.ActivityRecurrencePatterns
            .AsNoTracking()
            .Where(p => p.ProgramCode == programCode);

        if (!string.IsNullOrWhiteSpace(filter.StateName))
        {
            query = query.Where(p => p.StateCode == filter.StateName);
        }

        var patterns = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return patterns.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task AddAsync(ActivityRecurrencePattern pattern)
    {
        var model = DomainPersistenceMapper.ToPersistence(pattern);
        await _context.ActivityRecurrencePatterns.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ActivityRecurrencePattern pattern)
    {
        var existing = await _context.ActivityRecurrencePatterns
            .FirstOrDefaultAsync(p => p.UvaCode == pattern.UvaCode)
            ?? throw new InvalidOperationException("Patrón de recurrencia no encontrado para actualizar");

        existing.Name = pattern.Name;
        existing.RecurrenceType = pattern.RecurrenceType;
        existing.StateCode = pattern.StateCode;
        existing.UpdatedAt = pattern.UpdatedAt;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string uvaCode)
    {
        var pattern = await _context.ActivityRecurrencePatterns.FirstOrDefaultAsync(p => p.UvaCode == uvaCode);
        if (pattern != null)
        {
            _context.ActivityRecurrencePatterns.Remove(pattern);
            await _context.SaveChangesAsync();
        }
    }
}
