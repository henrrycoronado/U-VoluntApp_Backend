namespace U_VoluntApp_Core.Src.Application.DTOs;

using System;
using System.ComponentModel.DataAnnotations;

public class CreateRoleRequestDto
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El motivo es obligatorio.")]
    public string Reason { get; set; } = string.Empty;

    public int? DurationInMonths { get; set; }
}

public class RoleRequestResponseDto
{
    public string UvaCode { get; set; } = string.Empty;

    public string RequesterProfileCode { get; set; } = string.Empty;

    public string RequestedRole { get; set; } = string.Empty;

    public string Reason { get; set; } = string.Empty;

    public int? DurationInMonths { get; set; }

    public string StateCode { get; set; } = string.Empty;

    public string? ResolvedByProfileCode { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }
}
