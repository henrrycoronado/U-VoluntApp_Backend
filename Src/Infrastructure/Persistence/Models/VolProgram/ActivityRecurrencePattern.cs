namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.VolProgram;

using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Activity;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.States;

public partial class ActivityRecurrencePattern
{
    public long Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string ProgramCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string RecurrenceType { get; set; } = null!;

    public string StateCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual ICollection<ActivityRecurrenceDetail> ActivityRecurrenceDetails { get; set; } = new List<ActivityRecurrenceDetail>();

    public virtual VolProgram Program { get; set; } = null!;

    public virtual ActivityState State { get; set; } = null!;
}
