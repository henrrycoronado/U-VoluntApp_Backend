namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Profile;

public partial class MvVolunteerHistory
{
    public string? ProfileCode { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? CareerName { get; set; }

    public decimal? PersonalGoalHours { get; set; }

    public long? TotalActivitiesParticipated { get; set; }

    public decimal? ValidatedHours { get; set; }

    public decimal? TotalLoggedHours { get; set; }

    public DateTime? LastActivityDate { get; set; }
}
