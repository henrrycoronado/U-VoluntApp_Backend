namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Core.Src.Domain.Entities.VolProgram;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.VolProgram;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Mappers;

public class VolProgramContentRepository : IVolProgramContentRepository
{
    private readonly AppDbContext _context;

    public VolProgramContentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<VolProgramContent?> GetByProgramCodeAsync(string programCode)
    {
        var content = await _context.VolProgramContents
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ProgramCode == programCode);

        return content is null ? null : DomainPersistenceMapper.ToDomain(content);
    }

    public async Task AddAsync(VolProgramContent content)
    {
        var model = DomainPersistenceMapper.ToPersistence(content);
        await _context.VolProgramContents.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(VolProgramContent content)
    {
        var existing = await _context.VolProgramContents
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
