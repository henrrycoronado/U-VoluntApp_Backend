namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.VolProgram;

using U_VoluntApp_Core.Src.Domain.Entities.VolProgram;

public interface IVolProgramPatternDetailRepository
{
    Task<VolProgramPatternDetail?> GetByCodeAsync(string uvaCode);

    Task AddAsync(VolProgramPatternDetail detail);

    Task UpdateAsync(VolProgramPatternDetail detail);
}
