namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Types;

using U_VoluntApp_Backend.Src.Domain.Types;

public interface IEvidenceTypeRepository
{
    Task<IEnumerable<EvidenceType>> GetAllAsync();

    Task<EvidenceType?> GetByCodeAsync(string uvaCode);

    Task<EvidenceType?> GetByNameAsync(string name);

    Task AddAsync(EvidenceType type);

    Task UpdateAsync(EvidenceType type);

    Task DeleteAsync(string uvaCode);
}
