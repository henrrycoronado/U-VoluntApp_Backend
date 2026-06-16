namespace U_VoluntApp_Backend.Src.Application.DTOs;

using System.ComponentModel.DataAnnotations;

public class UpdateProfileDto
{
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    public string? FirstName { get; set; }

    [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres")]
    public string? LastName { get; set; }

    [Phone(ErrorMessage = "El formato del teléfono es inválido")]
    public string? Phone { get; set; }

    [StringLength(200, ErrorMessage = "La ubicación no puede exceder 200 caracteres")]
    public string? HousingLocation { get; set; }

    public string? CareerCode { get; set; }

    [Range(0, 10000, ErrorMessage = "Las horas personales deben estar entre 0 y 10000")]
    public decimal? PersonalGoalHours { get; set; }
}

public class ProfileResponseDto
{
    public string UvaCode { get; set; } = null!;

    public string IdentityUserId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhotoUrl { get; set; }

    public string? Phone { get; set; }

    public string? HousingLocation { get; set; }

    public string? CareerCode { get; set; }

    public string? CareerName { get; set; }

    public decimal PersonalGoalHours { get; set; }

    public string StateCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
