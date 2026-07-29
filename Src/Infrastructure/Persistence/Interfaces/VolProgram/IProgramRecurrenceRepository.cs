namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.VolProgram;

using U_VoluntApp_Core.Src.Domain.Entities.VolProgram;
using U_VoluntApp_Core.Src.Domain.Utils.Configuration;

public interface IVolProgramPatternRepository
{
    Task<VolProgramPattern?> GetByCodeAsync(string uvaCode);

    Task<IEnumerable<VolProgramPattern>> GetByProgramCodeAsync(string programCode, RequestFilter filter);

    Task AddAsync(VolProgramPattern pattern);

    Task UpdateAsync(VolProgramPattern pattern);

    Task DeleteAsync(string uvaCode);
}
