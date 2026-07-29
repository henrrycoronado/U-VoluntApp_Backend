namespace U_VoluntApp_Core.Src.Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U_VoluntApp_Core.Src.Application.DTOs;
using U_VoluntApp_Core.Src.Application.Interfaces;
using U_VoluntApp_Core.Src.Domain.Utils.Constants;
using U_VoluntApp_Core.Src.Presentation.Helpers;

[ApiController]
[Route("api/v1/activities")]
[Authorize]
public class ActivityController : ControllerBase
{
    private readonly IActivityService _activityService;

    public ActivityController(IActivityService activityService)
    {
        _activityService = activityService;
    }

    /// <summary>
    /// Crea una actividad nuevo.
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
    [Authorize(Roles = $"{RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> Create([FromBody] CreateActivityDto dto)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.AdminRole);
        var result = await _activityService.CreateAsync(dto, requesterId, requesterRole);
        return CreatedAtAction(nameof(GetByCode), new { uvaCode = result.UvaCode }, result);
    }

    /// <summary>
    /// Crea una actividad de forma simplificada.
    /// </summary>
    /// <param name="dto">El parametro dto.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost("simple")]
    [Authorize(Roles = $"{RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> CreateSimple([FromBody] CreateActivitySimpleDto dto)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.AdminRole);
        var result = await _activityService.CreateSimpleAsync(dto, requesterId, requesterRole);
        return CreatedAtAction(nameof(GetByCode), new { uvaCode = result.UvaCode }, result);
    }

    /// <summary>
    /// Obtiene los detalles de una actividad específico.
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
    [Authorize(Roles = $"{RoleConstants.VolunteerRole}, {RoleConstants.CoordinatorRole}, {RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> GetByCode(string uvaCode)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.VolunteerRole);
        var result = await _activityService.GetByCodeAsync(uvaCode, requesterId, requesterRole);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene la lista de actividades asociados a un programa.
    /// </summary>
    /// <param name="programCode">El parametro programCode.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("by-program/{programCode}")]
    [Authorize(Roles = $"{RoleConstants.VolunteerRole}, {RoleConstants.CoordinatorRole}, {RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> GetByProgram(string programCode)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.VolunteerRole);
        var result = await _activityService.GetByProgramAsync(programCode, requesterId, requesterRole);
        return Ok(result);
    }

    /// <summary>
    /// Actualiza los datos de una actividad existente.
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
    [Authorize(Roles = $"{RoleConstants.CoordinatorRole}, {RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> Update(string uvaCode, [FromBody] UpdateActivityDto dto)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.AdminRole);
        var result = await _activityService.UpdateAsync(uvaCode, dto, requesterId, requesterRole);
        return Ok(result);
    }

    /// <summary>
    /// Cambia el estado de una actividad.
    /// </summary>
    /// <param name="uvaCode">El parametro uvaCode.</param>
    /// <param name="dto">El parametro dto.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPatch("{uvaCode}/state")]
    [Authorize(Roles = $"{RoleConstants.CoordinatorRole}, {RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> ChangeState(string uvaCode, [FromBody] ChangeActivityStateDto dto)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.AdminRole);
        await _activityService.ChangeStateAsync(uvaCode, dto, requesterId, requesterRole);
        return NoContent();
    }

    /// <summary>
    /// Elimina una actividad del sistema.
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
    [Authorize(Roles = $"{RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> Delete(string uvaCode)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.AdminRole);
        await _activityService.DeleteAsync(uvaCode, requesterId, requesterRole);
        return NoContent();
    }
}
