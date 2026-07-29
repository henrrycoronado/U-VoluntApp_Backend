namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.VolProgram;

public partial class VolProgramPatternDetail
{
    public long Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string VolProgramPatternCode { get; set; } = null!;

    public short? DayOfWeek { get; set; }

    public short? DayOfMonth { get; set; }

    public short? WeekOfMonth { get; set; }

    public TimeOnly? StartHour { get; set; }

    public TimeOnly? EndHour { get; set; }

    public string StateCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual VolProgramPattern VolProgramPattern { get; set; } = null!;
}
