namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Interfaces.Activity;

using U_VoluntApp_Core.Src.Domain.Entities.Activity;

public interface IActivityRuleRepository
{
    Task<ActivityRule?> GetByActivityCodeAsync(string activityCode);

    Task AddAsync(ActivityRule rule);

    Task UpdateAsync(ActivityRule rule);
}
