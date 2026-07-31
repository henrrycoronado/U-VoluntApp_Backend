namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.VolProgram;

using U_VoluntApp_Core.Src.Domain.Entities.VolProgram;

public interface IVolProgramContentRepository
{
    Task<VolProgramContent?> GetByProgramCodeAsync(string programCode);

    Task AddAsync(VolProgramContent content);

    Task UpdateAsync(VolProgramContent content);
}
