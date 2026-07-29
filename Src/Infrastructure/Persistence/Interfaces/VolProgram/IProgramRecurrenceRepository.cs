namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.VolProgram;

using U_VoluntApp_Core.Src.Domain.Entities.VolProgram;
using U_VoluntApp_Core.Src.Domain.Utils.Configuration;

public interface IProgramRecurrenceRepository
{
    Task<ActivityRecurrencePattern?> GetByCodeAsync(string uvaCode);

    Task<IEnumerable<ActivityRecurrencePattern>> GetByProgramCodeAsync(string programCode, RequestFilter filter);

    Task AddAsync(ActivityRecurrencePattern pattern);

    Task UpdateAsync(ActivityRecurrencePattern pattern);

    Task DeleteAsync(string uvaCode);
}
