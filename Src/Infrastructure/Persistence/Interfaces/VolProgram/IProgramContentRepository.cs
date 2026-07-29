namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.VolProgram;

using U_VoluntApp_Core.Src.Domain.Entities.VolProgram;

public interface IProgramContentRepository
{
    Task<ProgramContent?> GetByProgramCodeAsync(string programCode);

    Task AddAsync(ProgramContent content);

    Task UpdateAsync(ProgramContent content);
}
