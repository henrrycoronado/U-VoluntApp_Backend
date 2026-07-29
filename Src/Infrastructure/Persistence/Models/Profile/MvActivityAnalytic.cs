namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Profile;

public partial class MvActivityAnalytic
{
    public string? ActivityCode { get; set; }

    public string? ProgramCode { get; set; }

    public string? ProgramName { get; set; }

    public string? ActivityName { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? TotalCapacity { get; set; }

    public long? TotalEnrolled { get; set; }

    public long? TotalAttended { get; set; }

    public decimal? TotalActivityHours { get; set; }
}
