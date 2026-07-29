namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.VolProgram;

using U_VoluntApp_Core.Src.Domain.Entities.VolProgram;

public interface IProgramDetailRecurrenceRepository
{
    Task<ActivityRecurrenceDetail?> GetByCodeAsync(string uvaCode);

    Task AddAsync(ActivityRecurrenceDetail detail);

    Task UpdateAsync(ActivityRecurrenceDetail detail);
}
