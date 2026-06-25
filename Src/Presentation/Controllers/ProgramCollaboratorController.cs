namespace U_VoluntApp_Backend.Src.Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;
using U_VoluntApp_Backend.Src.Presentation.Helpers;

[ApiController]
[Route("api/v1/collaborators")]
[Authorize]
public class ProgramCollaboratorController : ControllerBase
{
    private readonly IProgramCollaboratorService _collaboratorService;

    public ProgramCollaboratorController(IProgramCollaboratorService collaboratorService)
    {
        _collaboratorService = collaboratorService;
    }

    /// <summary>
    /// Procesa la acción Add para un colaborador.
    /// </summary>
    /// <param name="dto">El parametro dto.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    [Authorize(Roles = $"{RoleConstants.CoordinatorRole}, {RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> Add([FromBody] AddProgramCollaboratorDto dto)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.CoordinatorRole);
        var result = await _collaboratorService.AddAsync(dto, requesterId, requesterRole);
        return CreatedAtAction(nameof(GetByCode), new { uvaCode = result.UvaCode }, result);
    }

    /// <summary>
    /// Procesa la acción GetByProgramId para un colaborador.
    /// </summary>
    /// <param name="programCode">El parametro programCode.</param>
    /// <param name="stateCode">El parametro stateCode.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("program/{programCode}")]
    [Authorize(Roles = $"{RoleConstants.VolunteerRole}, {RoleConstants.CoordinatorRole}, {RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> GetByProgramId(string programCode, string stateCode)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.VolunteerRole);
        var result = await _collaboratorService.GetByProgramIdAsync(programCode, requesterId, requesterRole, stateCode);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene los detalles de un colaborador específico.
    /// </summary>
    /// <param name="uvaCode">El parametro uvaCode.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("{uvaCode}")]
    public async Task<IActionResult> GetByCode(string uvaCode)
    {
        var result = await _collaboratorService.GetByCodeAsync(uvaCode);
        if (result is null)
        {
            return NotFound(new
            {
                error = "Colaborador no encontrado",
                code = StatusCodes.Status404NotFound,
            });
        }

        return Ok(result);
    }

    /// <summary>
    /// Actualiza los datos de un colaborador existente.
    /// </summary>
    /// <param name="uvaCode">El parametro uvaCode.</param>
    /// <param name="dto">El parametro dto.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPut("{uvaCode}")]
    [Authorize(Roles = $"{RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> Update(string uvaCode, [FromBody] UpdateProgramCollaboratorDto dto)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.AdminRole);
        var result = await _collaboratorService.UpdateAsync(uvaCode, dto, requesterId, requesterRole);
        return Ok(result);
    }

    /// <summary>
    /// Elimina un colaborador del sistema.
    /// </summary>
    /// <param name="uvaCode">El parametro uvaCode.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpDelete("{uvaCode}")]
    [Authorize(Roles = $"{RoleConstants.CoordinatorRole}, {RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> Delete(string uvaCode)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.CoordinatorRole);
        await _collaboratorService.DeleteAsync(uvaCode, requesterId, requesterRole);
        return NoContent();
    }
}
