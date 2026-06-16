namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.VolProgram;

using U_VoluntApp_Backend.Src.Domain.Entities.VolProgram;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public interface IProgramCollaboratorRepository
{
    Task<ProgramCollaborator?> GetByCodeAsync(string uvaCode);

    Task<IEnumerable<ProgramCollaborator>> GetByProgramCodeAsync(string programCode, RequestFilter filter);

    Task<IEnumerable<ProgramCollaborator>> GetByProfileCodeAsync(string profileCode, RequestFilter filter);

    Task AddAsync(ProgramCollaborator collaborator);

    Task UpdateAsync(ProgramCollaborator collaborator);

    Task DeleteAsync(string uvaCode);
}
