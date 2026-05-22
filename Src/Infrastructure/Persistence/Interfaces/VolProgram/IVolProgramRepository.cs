namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.VolProgram;

using U_VoluntApp_Backend.Src.Domain.Entities.VolProgram;
using U_VoluntApp_Backend.Src.Domain.Utils.Configuration;

public interface IVolProgramRepository
{
    Task<VolProgram?> GetByCodeAsync(string uvaCode);

    Task<IEnumerable<VolProgram>> GetAllAsync(RequestFilter filter);

    Task<IEnumerable<VolProgram>> GetByManagerCodeAsync(string profileCode, RequestFilter filter);

    Task AddAsync(VolProgram program);

    Task UpdateAsync(VolProgram program);

    Task DeleteAsync(string uvaCode);
}
