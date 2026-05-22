namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Interfaces.Activity;

using U_VoluntApp_Backend.Src.Domain.Entities.Activity;

public interface IActivityRuleRepository
{
    Task<ActivityRule?> GetByActivityCodeAsync(string activityCode);

    Task AddAsync(ActivityRule rule);

    Task UpdateAsync(ActivityRule rule);
}
