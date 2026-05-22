namespace U_VoluntApp_Backend.Src.Domain.Entities.Profile;

public class ScholarshipPerformance
{
    public string ScholarshipCode { get; private set; } = string.Empty;

    public string ProfileCode { get; private set; } = string.Empty;

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public string ScholarshipType { get; private set; } = string.Empty;

    public decimal RequiredHours { get; private set; }

    public decimal CompletedHours { get; private set; }

    public decimal RemainingHours { get; private set; }

    public decimal CompletionPercentage { get; private set; }

    public string ContractState { get; private set; } = string.Empty;

    public DateTime? EndDate { get; private set; }

    internal static ScholarshipPerformance Rehydrate(
        string scholarshipCode,
        string profileCode,
        string firstName,
        string lastName,
        string scholarshipType,
        decimal requiredHours,
        decimal completedHours,
        decimal remainingHours,
        decimal completionPercentage,
        string contractState,
        DateTime? endDate)
    {
        return new ScholarshipPerformance
        {
            ScholarshipCode = scholarshipCode,
            ProfileCode = profileCode,
            FirstName = firstName,
            LastName = lastName,
            ScholarshipType = scholarshipType,
            RequiredHours = requiredHours,
            CompletedHours = completedHours,
            RemainingHours = remainingHours,
            CompletionPercentage = completionPercentage,
            ContractState = contractState,
            EndDate = endDate,
        };
    }
}
