namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.States;

using U_VoluntApp_Backend.Src.Domain.States;

public interface IEnrollmentStateRepository
{
    Task<IEnumerable<EnrollmentState>> GetAllAsync();

    Task<EnrollmentState?> GetByCodeAsync(string uvaCode);

    Task<EnrollmentState?> GetByNameAsync(string name);

    Task UpdateAsync(EnrollmentState state);
}
