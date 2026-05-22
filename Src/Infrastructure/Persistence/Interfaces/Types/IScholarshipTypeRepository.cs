namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Types;

using U_VoluntApp_Backend.Src.Domain.Types;

public interface IScholarshipTypeRepository
{
    Task<IEnumerable<ScholarshipType>> GetAllAsync();

    Task<ScholarshipType?> GetByCodeAsync(string uvaCode);

    Task<ScholarshipType?> GetByNameAsync(string name);

    Task AddAsync(ScholarshipType type);

    Task UpdateAsync(ScholarshipType type);

    Task DeleteAsync(string uvaCode);
}
