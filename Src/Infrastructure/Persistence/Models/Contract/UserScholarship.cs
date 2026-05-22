namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Contract;

using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Profile;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.States;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Types;

public partial class UserScholarship
{
    public long Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string AssignedProfileCode { get; set; } = null!;

    public string? EvaluatorProfileCode { get; set; }

    public string ScholarshipTypeCode { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public decimal RequiredHours { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string StateCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Profile AssignedProfile { get; set; } = null!;

    public virtual Profile? EvaluatorProfile { get; set; }

    public virtual ScholarshipType ScholarshipType { get; set; } = null!;

    public virtual ContractState State { get; set; } = null!;
}
