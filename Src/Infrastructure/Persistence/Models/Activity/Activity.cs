namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Activity;

using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Enrollment;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Profile;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.VolProgram;

public partial class Activity
{
    public long Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string ProgramCode { get; set; } = null!;

    public string? ResponsibleProfileCode { get; set; }

    public string ActivityTypeCode { get; set; } = null!;

    public string? ActivityRecurrencePatternCode { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public double LocationLatitude { get; set; }

    public double LocationLongitude { get; set; }

    public int RegistrationRadiusMeters { get; set; }

    public string StateCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<ActivityGroup> ActivityGroups { get; set; } = new List<ActivityGroup>();

    public virtual ActivityRecurrencePattern? ActivityRecurrencePattern { get; set; }

    public virtual ActivityRule? ActivityRule { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual VolProgram Program { get; set; } = null!;

    public virtual Profile? ResponsibleProfile { get; set; }
}
