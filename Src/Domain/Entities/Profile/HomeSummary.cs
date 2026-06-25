namespace U_VoluntApp_Backend.Src.Domain.Entities.Profile;

public class DailyActivity
{
    public int Day { get; private set; }

    public decimal Hours { get; private set; }

    public static DailyActivity Create(int day, decimal hours)
    {
        return new DailyActivity { Day = day, Hours = hours };
    }
}

public class HomeSummary
{
    public decimal PersonalGoalHours { get; private set; }

    public decimal ScholarshipGoalHours { get; private set; }

    public decimal MonthLoggedHours { get; private set; }

    public decimal TotalLoggedHours { get; private set; }

    public List<DailyActivity> CurrentMonthDailyActivities { get; private set; } = new List<DailyActivity>();

    public static HomeSummary Create(
        decimal personalGoalHours,
        decimal scholarshipGoalHours,
        decimal monthLoggedHours,
        decimal totalLoggedHours,
        List<DailyActivity> currentMonthDailyActivities)
    {
        return new HomeSummary
        {
            PersonalGoalHours = personalGoalHours,
            ScholarshipGoalHours = scholarshipGoalHours,
            MonthLoggedHours = monthLoggedHours,
            TotalLoggedHours = totalLoggedHours,
            CurrentMonthDailyActivities = currentMonthDailyActivities
        };
    }
}
