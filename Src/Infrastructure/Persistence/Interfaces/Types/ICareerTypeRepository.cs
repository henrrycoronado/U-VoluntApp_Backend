namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Types;

using U_VoluntApp_Backend.Src.Domain.Types;

public interface ICareerTypeRepository
{
    Task<IEnumerable<CareerType>> GetAllAsync();

    Task<CareerType?> GetByCodeAsync(string uvaCode);

    Task<CareerType?> GetByNameAsync(string name);

    Task AddAsync(CareerType career);

    Task UpdateAsync(CareerType career);

    Task DeleteAsync(string uvaCode);
}
