namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.States;

using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Activity;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Enrollment;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.VolProgram;

public partial class ActivityState
{
    public int Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual ICollection<ActivityGroup> ActivityGroups { get; set; } = new List<ActivityGroup>();

    public virtual ICollection<ActivityRecurrencePattern> ActivityRecurrencePatterns { get; set; } = new List<ActivityRecurrencePattern>();

    public virtual ICollection<ActivityRecurrenceDetail> ActivityRecurrenceDetails { get; set; } = new List<ActivityRecurrenceDetail>();

    public virtual ICollection<GroupEnrollment> GroupEnrollments { get; set; } = new List<GroupEnrollment>();
}
