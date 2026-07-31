namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Core.Src.Domain.Entities.VolProgram;
using U_VoluntApp_Core.Src.Domain.Utils.Configuration;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.VolProgram;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Mappers;

public class VolProgramCollaboratorRepository : IVolProgramCollaboratorRepository
{
    private readonly AppDbContext _context;

    public VolProgramCollaboratorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<VolProgramCollaborator?> GetByCodeAsync(string uvaCode)
    {
        var collaborator = await _context.VolProgramCollaborators
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UvaCode == uvaCode);

        return collaborator is null ? null : DomainPersistenceMapper.ToDomain(collaborator);
    }

    public async Task<IEnumerable<VolProgramCollaborator>> GetByProgramCodeAsync(string programCode, RequestFilter filter)
    {
        var collaborators = await _context.VolProgramCollaborators
            .AsNoTracking()
            .Where(c => c.ProgramCode == programCode)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return collaborators.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<VolProgramCollaborator>> GetByProfileCodeAsync(string profileCode, RequestFilter filter)
    {
        var collaborators = await _context.VolProgramCollaborators
            .AsNoTracking()
            .Where(c => c.ProfileCode == profileCode)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return collaborators.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task AddAsync(VolProgramCollaborator collaborator)
    {
        var model = DomainPersistenceMapper.ToPersistence(collaborator);
        await _context.VolProgramCollaborators.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(VolProgramCollaborator collaborator)
    {
        var existing = await _context.VolProgramCollaborators
            .FirstOrDefaultAsync(c => c.UvaCode == collaborator.UvaCode)
            ?? throw new InvalidOperationException("Colaborador no encontrado para actualizar");

        existing.StateCode = collaborator.StateCode;
        existing.AssignedByProfileCode = collaborator.AssignedByProfileCode;
        existing.UpdatedAt = collaborator.UpdatedAt;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string uvaCode)
    {
        var collaborator = await _context.VolProgramCollaborators.FirstOrDefaultAsync(c => c.UvaCode == uvaCode);
        if (collaborator != null)
        {
            _context.VolProgramCollaborators.Remove(collaborator);
            await _context.SaveChangesAsync();
        }
    }
}
