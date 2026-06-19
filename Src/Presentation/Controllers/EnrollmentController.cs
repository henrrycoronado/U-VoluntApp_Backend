namespace U_VoluntApp_Backend.Src.Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;
using U_VoluntApp_Backend.Src.Presentation.Helpers;

[ApiController]
[Route("api/v1/enrollments")]
[Authorize]
public class EnrollmentController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    /// <summary>
    /// Procesa la acción Enroll para una inscripción.
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
    public async Task<IActionResult> Enroll([FromBody] CreateEnrollmentDto dto)
    {
        var profileCode = ControllerHelper.GetProfileId(User); // Assuming this returns uva_code
        var result = await _enrollmentService.EnrollAsync(dto, profileCode);
        return CreatedAtAction(nameof(GetByCode), new { uvaCode = result.UvaCode }, result);
    }

    /// <summary>
    /// Obtiene los detalles de una inscripción específico.
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
        var result = await _enrollmentService.GetByCodeAsync(uvaCode);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción GetByActivity para una inscripción.
    /// </summary>
    /// <param name="activityCode">El parametro activityCode.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("by-activity/{activityCode}")]
    public async Task<IActionResult> GetByActivity(string activityCode)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.VolunteerRole);
        var result = await _enrollmentService.GetByActivityAsync(activityCode, requesterId, requesterRole);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción GetMyEnrollments para una inscripción.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("mine")]
    public async Task<IActionResult> GetMyEnrollments()
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        var result = await _enrollmentService.GetMyEnrollmentsAsync(profileCode);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción Review para una inscripción.
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
    [HttpPatch("{uvaCode}/review")]
    public async Task<IActionResult> Review(string uvaCode, [FromBody] ReviewEnrollmentDto dto)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.CoordinatorRole);
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.CoordinatorRole);
        await _enrollmentService.ReviewAsync(uvaCode, dto, requesterId, requesterRole);
        return NoContent();
    }

    /// <summary>
    /// Procesa la acción Cancel para una inscripción.
    /// </summary>
    /// <param name="uvaCode">El parametro uvaCode.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPatch("{uvaCode}/cancel")]
    public async Task<IActionResult> Cancel(string uvaCode)
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        await _enrollmentService.CancelAsync(uvaCode, profileCode);
        return NoContent();
    }
}
