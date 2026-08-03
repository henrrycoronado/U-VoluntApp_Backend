namespace U_VoluntApp_Core.Src.Application.DTOs;

public class ScholarshipAlertDto
{
    public required string ProfileCode { get; set; }

    public required string FullName { get; set; }

    public string? AvatarUrl { get; set; }

    public required string ScholarshipType { get; set; }

    public required string AlertReason { get; set; }
}
