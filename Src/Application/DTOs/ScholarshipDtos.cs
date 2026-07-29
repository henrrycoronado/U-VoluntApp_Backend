namespace U_VoluntApp_Core.Src.Application.DTOs;

using System.ComponentModel.DataAnnotations;

public class CreateScholarshipRequestDto
{
    [Required(ErrorMessage = "El tipo de beca es requerido")]
    public string ScholarshipTypeCode { get; set; } = null!;

    [Required(ErrorMessage = "La razón es requerida")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "La razón debe tener entre 10 y 500 caracteres")]
    public string Reason { get; set; } = null!;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}

public class CreateScholarshipForProfileDto
{
    [Required(ErrorMessage = "El ID del perfil es requerido")]
    public string ProfileCode { get; set; } = null!;

    [Required(ErrorMessage = "El tipo de beca es requerido")]
    public string ScholarshipTypeCode { get; set; } = null!;

    [Required(ErrorMessage = "La razón es requerida")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "La razón debe tener entre 10 y 500 caracteres")]
    public string Reason { get; set; } = null!;

    [Required(ErrorMessage = "Las horas requeridas son obligatorias")]
    [Range(0.5, 10000, ErrorMessage = "Las horas deben estar entre 0.5 y 10000")]
    public decimal RequiredHours { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}

public class ReviewScholarshipDto
{
    public bool Approve { get; set; }

    public decimal? RequiredHours { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}

public class CompleteScholarshipDto
{
    public DateTime? EndDate { get; set; }
}

public class ScholarshipResponseDto
{
    public string UvaCode { get; set; } = null!;

    public string ProfileCode { get; set; } = null!;

    public string VolunteerName { get; set; } = null!;

    public string ScholarshipTypeCode { get; set; } = null!;

    public string ScholarshipType { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public decimal RequiredHours { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string StateCode { get; set; } = null!;

    public string? EvaluatorProfileCode { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
