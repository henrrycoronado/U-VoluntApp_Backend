namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.VolProgram;

using U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Profile;

public partial class ProgramCollaborator
{
    public long Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string ProgramCode { get; set; } = null!;

    public string ProfileCode { get; set; } = null!;

    public string? AssignedByProfileCode { get; set; }

    public string StateCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Profile? AssignedByProfile { get; set; }

    public virtual Profile Profile { get; set; } = null!;

    public virtual VolProgram Program { get; set; } = null!;
}
