namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Enrollment;

using U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Activity;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Tracking;

public partial class GroupEnrollment
{
    public long Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string ActivityGroupCode { get; set; } = null!;

    public string EnrollmentCode { get; set; } = null!;

    public string StateCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ActivityGroup ActivityGroup { get; set; } = null!;

    public virtual Enrollment Enrollment { get; set; } = null!;

    public virtual ICollection<TrackingLog> TrackingLogs { get; set; } = new List<TrackingLog>();
}
