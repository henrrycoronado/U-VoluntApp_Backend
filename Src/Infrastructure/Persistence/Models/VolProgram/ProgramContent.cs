namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.VolProgram;

public partial class ProgramContent
{
    public long Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string ProgramCode { get; set; } = null!;

    public string? Description { get; set; }

    public string? ActivitiesDescription { get; set; }

    public string? ScheduleInfo { get; set; }

    public string? LeadershipInfo { get; set; }

    public string? ContactInfo { get; set; }

    public string? MissionStatement { get; set; }

    public string? ProfilePhotoUrl { get; set; }

    public string? CoverPhotoUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual VolProgram Program { get; set; } = null!;
}
