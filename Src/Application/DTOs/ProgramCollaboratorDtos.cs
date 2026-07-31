namespace U_VoluntApp_Core.Src.Application.DTOs;

public class AddVolProgramCollaboratorDto
{
    public string ProgramCode { get; set; } = null!;

    public string ProfileCode { get; set; } = null!;

    public string StateCode { get; set; } = null!;
}

public class UpdateVolProgramCollaboratorDto
{
    public string StateCode { get; set; } = null!;
}

public class VolProgramCollaboratorResponseDto
{
    public string UvaCode { get; set; } = null!;

    public string ProgramCode { get; set; } = null!;

    public string ProfileCode { get; set; } = null!;

    public string? ProfileName { get; set; }

    public string? AssignedByProfileCode { get; set; }

    public string? AssignedByName { get; set; }

    public string StateCode { get; set; } = null!;

    public string? StateName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

public class VolProgramCollaboratorListDto
{
    public IEnumerable<VolProgramCollaboratorResponseDto> VolProgramCollaborators { get; set; } = new List<VolProgramCollaboratorResponseDto>();

    public int Total { get; set; }
}
