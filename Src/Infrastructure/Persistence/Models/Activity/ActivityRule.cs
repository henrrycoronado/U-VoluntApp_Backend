namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Activity;

public partial class ActivityRule
{
    public long Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string ActivityCode { get; set; } = null!;

    public bool RequiresEnrollment { get; set; }

    public DateTime? EnrollmentDeadline { get; set; }

    public bool RequiresApproval { get; set; }

    public int? TotalCapacity { get; set; }

    public decimal CostAmount { get; set; }

    public bool CountsVolunteerHours { get; set; }

    public string? PhotoUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Activity Activity { get; set; } = null!;
}
