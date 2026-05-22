namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.States;

using U_VoluntApp_Backend.Src.Domain.States;

public interface IProgramStateRepository
{
    Task<IEnumerable<ProgramState>> GetAllAsync();

    Task<ProgramState?> GetByCodeAsync(string uvaCode);

    Task<ProgramState?> GetByNameAsync(string name);

    Task UpdateAsync(ProgramState state);
}
