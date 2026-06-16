namespace U_VoluntApp_Backend.Src.Domain.Entities.Profile;

public class ProgramAnalytics
{
    public string ProgramCode { get; private set; } = string.Empty;

    public string ProgramName { get; private set; } = string.Empty;

    public long TotalActivities { get; private set; }

    public long TotalUniqueVolunteers { get; private set; }

    public decimal TotalGeneratedHours { get; private set; }

    internal static ProgramAnalytics Rehydrate(
        string programCode,
        string programName,
        long totalActivities,
        long totalUniqueVolunteers,
        decimal totalGeneratedHours)
    {
        return new ProgramAnalytics
        {
            ProgramCode = programCode,
            ProgramName = programName,
            TotalActivities = totalActivities,
            TotalUniqueVolunteers = totalUniqueVolunteers,
            TotalGeneratedHours = totalGeneratedHours,
        };
    }
}
