namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Profile;

public partial class MvProgramAnalytic
{
    public string? ProgramCode { get; set; }

    public string? ProgramName { get; set; }

    public long? TotalActivities { get; set; }

    public long? TotalUniqueVolunteers { get; set; }

    public decimal? TotalGeneratedHours { get; set; }
}
