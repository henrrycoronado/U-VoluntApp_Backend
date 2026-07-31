namespace U_VoluntApp_Core.Src.Application.DTOs;

using System.ComponentModel.DataAnnotations;

public class CreateVolProgramDto
{
    [Required(ErrorMessage = "El nombre del programa es requerido")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 200 caracteres")]
    public string Name { get; set; } = null!;

    [StringLength(10, ErrorMessage = "El acrónimo no puede exceder 10 caracteres")]
    public string? Acronym { get; set; }
}

public class UpdateVolProgramDto
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Acronym { get; set; }

    public string? Color { get; set; }

    public string? ProfilePhotoUrl { get; set; }

    public string? CoverPhotoUrl { get; set; }

    public string? MissionStatement { get; set; }

    public string? ScheduleInfo { get; set; }

    public string? ContactInfo { get; set; }

    public string? LeadershipInfo { get; set; }
}

public class VolProgramResponseDto
{
    public string UvaCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Acronym { get; set; }

    public string? Color { get; set; }

    public string? ProfilePhotoUrl { get; set; }

    public string? CoverPhotoUrl { get; set; }

    public string? MissionStatement { get; set; }

    public string? ScheduleInfo { get; set; }

    public string? ContactInfo { get; set; }

    public string? LeadershipInfo { get; set; }

    public string ManagerProfileId { get; set; } = null!;

    public string ManagerName { get; set; } = null!;

    public string State { get; set; } = null!;

    public string StateCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}

public class ChangeVolProgramStateDto
{
    public string StateCode { get; set; } = null!;
}
