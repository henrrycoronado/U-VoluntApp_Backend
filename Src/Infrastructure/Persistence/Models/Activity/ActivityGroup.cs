namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Activity;

using U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Enrollment;

public partial class ActivityGroup
{
    public long Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string ActivityCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Details { get; set; }

    public int? TotalCapacity { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string StateCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Activity Activity { get; set; } = null!;

    public virtual ICollection<GroupEnrollment> GroupEnrollments { get; set; } = new List<GroupEnrollment>();
}
