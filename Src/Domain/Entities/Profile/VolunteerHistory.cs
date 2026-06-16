namespace U_VoluntApp_Backend.Src.Domain.Entities.Profile;

public class VolunteerHistory
{
    public string ProfileCode { get; private set; } = string.Empty;

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public string? CareerName { get; private set; }

    public decimal PersonalGoalHours { get; private set; }

    public long TotalActivitiesParticipated { get; private set; }

    public decimal ValidatedHours { get; private set; }

    public decimal TotalLoggedHours { get; private set; }

    public DateTime? LastActivityDate { get; private set; }

    internal static VolunteerHistory Rehydrate(
        string profileCode,
        string firstName,
        string lastName,
        string? careerName,
        decimal personalGoalHours,
        long totalActivitiesParticipated,
        decimal validatedHours,
        decimal totalLoggedHours,
        DateTime? lastActivityDate)
    {
        return new VolunteerHistory
        {
            ProfileCode = profileCode,
            FirstName = firstName,
            LastName = lastName,
            CareerName = careerName,
            PersonalGoalHours = personalGoalHours,
            TotalActivitiesParticipated = totalActivitiesParticipated,
            ValidatedHours = validatedHours,
            TotalLoggedHours = totalLoggedHours,
            LastActivityDate = lastActivityDate,
        };
    }
}
