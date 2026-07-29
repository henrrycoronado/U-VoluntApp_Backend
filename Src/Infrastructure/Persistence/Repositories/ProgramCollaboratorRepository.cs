namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Core.Src.Domain.Entities.VolProgram;
using U_VoluntApp_Core.Src.Domain.Utils.Configuration;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.VolProgram;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Mappers;

public class ProgramCollaboratorRepository : IProgramCollaboratorRepository
{
    private readonly AppDbContext _context;

    public ProgramCollaboratorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProgramCollaborator?> GetByCodeAsync(string uvaCode)
    {
        var collaborator = await _context.ProgramCollaborators
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UvaCode == uvaCode);

        return collaborator is null ? null : DomainPersistenceMapper.ToDomain(collaborator);
    }

    public async Task<IEnumerable<ProgramCollaborator>> GetByProgramCodeAsync(string programCode, RequestFilter filter)
    {
        var collaborators = await _context.ProgramCollaborators
            .AsNoTracking()
            .Where(c => c.ProgramCode == programCode)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return collaborators.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<ProgramCollaborator>> GetByProfileCodeAsync(string profileCode, RequestFilter filter)
    {
        var collaborators = await _context.ProgramCollaborators
            .AsNoTracking()
            .Where(c => c.ProfileCode == profileCode)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return collaborators.Select(DomainPersistenceMapper.ToDomain).ToList();
    }

    public async Task AddAsync(ProgramCollaborator collaborator)
    {
        var model = DomainPersistenceMapper.ToPersistence(collaborator);
        await _context.ProgramCollaborators.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProgramCollaborator collaborator)
    {
        var existing = await _context.ProgramCollaborators
            .FirstOrDefaultAsync(c => c.UvaCode == collaborator.UvaCode)
            ?? throw new InvalidOperationException("Colaborador no encontrado para actualizar");

        existing.StateCode = collaborator.StateCode;
        existing.AssignedByProfileCode = collaborator.AssignedByProfileCode;
        existing.UpdatedAt = collaborator.UpdatedAt;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string uvaCode)
    {
        var collaborator = await _context.ProgramCollaborators.FirstOrDefaultAsync(c => c.UvaCode == uvaCode);
        if (collaborator != null)
        {
            _context.ProgramCollaborators.Remove(collaborator);
            await _context.SaveChangesAsync();
        }
    }
}
