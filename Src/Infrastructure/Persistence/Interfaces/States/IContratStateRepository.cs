namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.States;

using U_VoluntApp_Backend.Src.Domain.States;

public interface IContractStateRepository
{
    Task<IEnumerable<ContractState>> GetAllAsync();

    Task<ContractState?> GetByCodeAsync(string uvaCode);

    Task<ContractState?> GetByNameAsync(string name);

    Task UpdateAsync(ContractState state);
}
