namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Backend.Src.Domain.Entities.VolProgram;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.VolProgram;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Mappers;

public class ProgramContentRepository : IProgramContentRepository
{
    private readonly AppDbContext _context;

    public ProgramContentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProgramContent?> GetByProgramCodeAsync(string programCode)
    {
        var content = await _context.ProgramContents
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ProgramCode == programCode);

        return content is null ? null : DomainPersistenceMapper.ToDomain(content);
    }

    public async Task AddAsync(ProgramContent content)
    {
        var model = DomainPersistenceMapper.ToPersistence(content);
        await _context.ProgramContents.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProgramContent content)
    {
        var existing = await _context.ProgramContents
            .FirstOrDefaultAsync(c => c.UvaCode == content.UvaCode)
            ?? throw new InvalidOperationException("Contenido de programa no encontrado para actualizar");

        existing.Description = content.Description;
        existing.ActivitiesDescription = content.ActivitiesDescription;
        existing.ScheduleInfo = content.ScheduleInfo;
        existing.LeadershipInfo = content.LeadershipInfo;
        existing.ContactInfo = content.ContactInfo;
        existing.MissionStatement = content.MissionStatement;
        existing.ProfilePhotoUrl = content.ProfilePhotoUrl;
        existing.CoverPhotoUrl = content.CoverPhotoUrl;
        existing.UpdatedAt = content.UpdatedAt;

        await _context.SaveChangesAsync();
    }
}
