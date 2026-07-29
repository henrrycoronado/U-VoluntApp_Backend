namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Profile;

public partial class MvScholarshipPerformance
{
    public string? ScholarshipCode { get; set; }

    public string? ProfileCode { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? ScholarshipType { get; set; }

    public decimal? RequiredHours { get; set; }

    public decimal? CompletedHours { get; set; }

    public decimal? RemainingHours { get; set; }

    public decimal? CompletionPercentage { get; set; }

    public string? ContractState { get; set; }

    public DateTime? EndDate { get; set; }
}
