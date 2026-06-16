namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.VolProgram;

public partial class ActivityRecurrenceDetail
{
    public long Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string ActivityRecurrencePatternCode { get; set; } = null!;

    public short? DayOfWeek { get; set; }

    public short? DayOfMonth { get; set; }

    public short? WeekOfMonth { get; set; }

    public TimeOnly? StartHour { get; set; }

    public TimeOnly? EndHour { get; set; }

    public string StateCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ActivityRecurrencePattern ActivityRecurrencePattern { get; set; } = null!;
}
