namespace U_VoluntApp_Core.Src.Domain.Entities.Profile;

public class ActivityAnalytics
{
    public string ActivityCode { get; private set; } = string.Empty;

    public string ProgramCode { get; private set; } = string.Empty;

    public string ProgramName { get; private set; } = string.Empty;

    public string ActivityName { get; private set; } = string.Empty;

    public DateTime StartDate { get; private set; }

    public DateTime EndDate { get; private set; }

    public int TotalCapacity { get; private set; }

    public long TotalEnrolled { get; private set; }

    public long TotalAttended { get; private set; }

    public decimal TotalActivityHours { get; private set; }

    internal static ActivityAnalytics Rehydrate(
        string activityCode,
        string programCode,
        string programName,
        string activityName,
        DateTime startDate,
        DateTime endDate,
        int totalCapacity,
        long totalEnrolled,
        long totalAttended,
        decimal totalActivityHours)
    {
        return new ActivityAnalytics
        {
            ActivityCode = activityCode,
            ProgramCode = programCode,
            ProgramName = programName,
            ActivityName = activityName,
            StartDate = startDate,
            EndDate = endDate,
            TotalCapacity = totalCapacity,
            TotalEnrolled = totalEnrolled,
            TotalAttended = totalAttended,
            TotalActivityHours = totalActivityHours,
        };
    }
}
