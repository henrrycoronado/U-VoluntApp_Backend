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
        var model = DomainPersistenceMapper.ToPersistence(detail);
        _context.ActivityRecurrenceDetails.Update(model);
        await _context.SaveChangesAsync();
    }
}
