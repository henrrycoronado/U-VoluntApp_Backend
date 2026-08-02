namespace U_VoluntApp_Core.Src.Application.DTOs;

public class ScholarshipPerformanceDto
{
    public string ProfileCode { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string ScholarshipType { get; set; } = null!;

    public decimal RequiredHours { get; set; }

    public decimal CompletedHours { get; set; }

    public decimal RemainingHours { get; set; }

    public decimal CompletionPercentage { get; set; }

    public string ContractState { get; set; } = null!;

    public DateTime? EndDate { get; set; }
}

public class ProgramAnalyticsDto
{
    public string ProgramCode { get; set; } = null!;

    public string ProgramName { get; set; } = null!;

    public long TotalActivities { get; set; }

    public long TotalUniqueVolunteers { get; set; }

    public decimal TotalGeneratedHours { get; set; }
}

public class ActivityAnalyticsDto
{
    public string ActivityCode { get; set; } = null!;

    public string ProgramCode { get; set; } = null!;

    public string ProgramName { get; set; } = null!;

    public string ActivityName { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int TotalCapacity { get; set; }

    public long TotalEnrolled { get; set; }

    public long TotalAttended { get; set; }

    public decimal TotalActivityHours { get; set; }
}

public class VolunteerHistoryDto
{
    public string ProfileCode { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? CareerName { get; set; }

    public decimal PersonalGoalHours { get; set; }

    public long TotalActivitiesParticipated { get; set; }

    public decimal ValidatedHours { get; set; }

    public decimal TotalLoggedHours { get; set; }

    public DateTime? LastActivityDate { get; set; }
}

public class DailyActivityDto
{
    public int Day { get; set; }

    public decimal Hours { get; set; }
}

public class HomeSummaryDto
{
    public decimal PersonalGoalHours { get; set; }

    public decimal ScholarshipGoalHours { get; set; }

    public decimal MonthLoggedHours { get; set; }

    public decimal TotalLoggedHours { get; set; }

    public List<DailyActivityDto> CurrentMonthDailyActivities { get; set; } = new List<DailyActivityDto>();
}

public class AdminHomeSummaryDto
{
    public long TotalVolunteers { get; set; }

    public decimal MonthlyLoggedHours { get; set; }

    public long ActivePrograms { get; set; }

    public long ActiveScholarships { get; set; }
}
