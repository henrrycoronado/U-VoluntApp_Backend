namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Backend.Src.Domain.Entities.VolProgram;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.VolProgram;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Mappers;

public class ProgramDetailRecurrenceRepository : IProgramDetailRecurrenceRepository
{
    private readonly AppDbContext _context;

    public ProgramDetailRecurrenceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ActivityRecurrenceDetail?> GetByCodeAsync(string uvaCode)
    {
        var detail = await _context.ActivityRecurrenceDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.UvaCode == uvaCode);

        return detail is null ? null : DomainPersistenceMapper.ToDomain(detail);
    }

    public async Task AddAsync(ActivityRecurrenceDetail detail)
    {
        var model = DomainPersistenceMapper.ToPersistence(detail);
        await _context.ActivityRecurrenceDetails.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ActivityRecurrenceDetail detail)
    {
        var existing = await _context.ActivityRecurrenceDetails
            .FirstOrDefaultAsync(d => d.UvaCode == detail.UvaCode)
            ?? throw new InvalidOperationException("Detalle de recurrencia no encontrado para actualizar");

        existing.DayOfWeek = detail.DayOfWeek;
        existing.DayOfMonth = detail.DayOfMonth;
        existing.WeekOfMonth = detail.WeekOfMonth;
        existing.StartHour = detail.StartHour;
        existing.EndHour = detail.EndHour;
        existing.StateCode = detail.StateCode;
        existing.UpdatedAt = detail.UpdatedAt;

        await _context.SaveChangesAsync();
    }
}
