namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Enrollment;

using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Activity;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Profile;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Tracking;

public partial class Enrollment
{
    public long Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string ActivityCode { get; set; } = null!;

    public string EnrolledProfileCode { get; set; } = null!;

    public string StateCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Activity Activity { get; set; } = null!;

    public virtual Profile EnrolledProfile { get; set; } = null!;

    public virtual ICollection<GroupEnrollment> GroupEnrollments { get; set; } = new List<GroupEnrollment>();

    public virtual ICollection<TrackingLog> TrackingLogs { get; set; } = new List<TrackingLog>();
}
