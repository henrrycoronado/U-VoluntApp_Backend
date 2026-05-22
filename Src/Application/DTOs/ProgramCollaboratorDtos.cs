namespace U_VoluntApp_Backend.Src.Application.DTOs;

public class AddProgramCollaboratorDto
{
    public string ProgramCode { get; set; } = null!;

    public string ProfileCode { get; set; } = null!;

    public string StateCode { get; set; } = null!;
}

public class UpdateProgramCollaboratorDto
{
    public string StateCode { get; set; } = null!;
}

public class ProgramCollaboratorResponseDto
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

public class ProgramCollaboratorListDto
{
    public IEnumerable<ProgramCollaboratorResponseDto> Collaborators { get; set; } = new List<ProgramCollaboratorResponseDto>();

    public int Total { get; set; }
}
