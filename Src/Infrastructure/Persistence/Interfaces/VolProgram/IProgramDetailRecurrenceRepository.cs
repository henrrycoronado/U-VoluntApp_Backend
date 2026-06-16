namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.VolProgram;

using U_VoluntApp_Backend.Src.Domain.Entities.VolProgram;

public interface IProgramDetailRecurrenceRepository
{
    Task<ActivityRecurrenceDetail?> GetByCodeAsync(string uvaCode);

    Task AddAsync(ActivityRecurrenceDetail detail);

    Task UpdateAsync(ActivityRecurrenceDetail detail);
}
