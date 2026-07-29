namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.VolProgram;

using U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Activity;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Profile;

public partial class VolProgram
{
    public long Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Acronym { get; set; }

    public string? ManagerProfileCode { get; set; }

    public string StateCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual ICollection<ActivityRecurrencePattern> ActivityRecurrencePatterns { get; set; } = new List<ActivityRecurrencePattern>();

    public virtual Profile? ManagerProfile { get; set; }

    public virtual ICollection<ProgramCollaborator> ProgramCollaborators { get; set; } = new List<ProgramCollaborator>();

    public virtual ProgramContent? ProgramContent { get; set; }
}
