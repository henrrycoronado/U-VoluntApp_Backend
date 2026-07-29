namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Tracking;

using U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Enrollment;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Profile;

public partial class TrackingLog
{
    public long Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string EnrollmentCode { get; set; } = null!;

    public string? GroupEnrollmentCode { get; set; }

    public DateTime? EntryTime { get; set; }

    public DateTime? ExitTime { get; set; }

    public decimal CalculatedHours { get; set; }

    public string StateCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? CheckInRegisteredByCode { get; set; }

    public string? CheckOutRegisteredByCode { get; set; }

    public virtual Profile? CheckInRegisteredBy { get; set; }

    public virtual Profile? CheckOutRegisteredBy { get; set; }

    public virtual GroupEnrollment? GroupEnrollment { get; set; }

    public virtual Enrollment Enrollment { get; set; } = null!;

    public virtual ICollection<Evidence> Evidences { get; set; } = new List<Evidence>();
}
