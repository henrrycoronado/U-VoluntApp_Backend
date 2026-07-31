namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Core.Src.Domain.Entities.VolProgram;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.VolProgram;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Mappers;

public class VolProgramPatternDetailRepository : IVolProgramPatternDetailRepository
{
    private readonly AppDbContext _context;

    public VolProgramPatternDetailRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<VolProgramPatternDetail?> GetByCodeAsync(string uvaCode)
    {
        var detail = await _context.VolProgramPatternDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.UvaCode == uvaCode);

        return detail is null ? null : DomainPersistenceMapper.ToDomain(detail);
    }

    public async Task AddAsync(VolProgramPatternDetail detail)
    {
        var model = DomainPersistenceMapper.ToPersistence(detail);
        await _context.VolProgramPatternDetails.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(VolProgramPatternDetail detail)
    {
        var existing = await _context.VolProgramPatternDetails
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
