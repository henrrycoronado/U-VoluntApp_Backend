namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.VolProgram;

using U_VoluntApp_Core.Src.Domain.Entities.VolProgram;
using U_VoluntApp_Core.Src.Domain.Utils.Configuration;

public interface IVolProgramCollaboratorRepository
{
    Task<VolProgramCollaborator?> GetByCodeAsync(string uvaCode);

    Task<IEnumerable<VolProgramCollaborator>> GetByProgramCodeAsync(string programCode, RequestFilter filter);

    Task<IEnumerable<VolProgramCollaborator>> GetByProfileCodeAsync(string profileCode, RequestFilter filter);

    Task AddAsync(VolProgramCollaborator collaborator);

    Task UpdateAsync(VolProgramCollaborator collaborator);

    Task DeleteAsync(string uvaCode);
}
