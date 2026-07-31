namespace U_VoluntApp_Core.Src.Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U_VoluntApp_Core.Src.Application.DTOs;
using U_VoluntApp_Core.Src.Application.Interfaces;
using U_VoluntApp_Core.Src.Domain.Utils.Constants;
using U_VoluntApp_Core.Src.Presentation.Helpers;

[ApiController]
[Route("api/v1/tracking")]
[Authorize]
public class TrackingController : ControllerBase
{
    private readonly ITrackingService _trackingService;

    public TrackingController(ITrackingService trackingService)
    {
        _trackingService = trackingService;
    }

    /// <summary>
    /// Procesa la acción CheckIn para un registro de tracking.
    /// </summary>
    /// <param name="dto">El parametro dto.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost("checkin")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CheckIn([FromForm] CheckInDto dto)
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        var result = await _trackingService.CheckInAsync(dto, profileCode);
        return CreatedAtAction(nameof(GetByCode), new { uvaCode = result.UvaCode }, result);
    }

    /// <summary>
    /// Procesa la acción CheckOut para un registro de tracking.
    /// </summary>
    /// <param name="dto">El parametro dto.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost("checkout")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CheckOut([FromForm] CheckOutDto dto)
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        var result = await _trackingService.CheckOutAsync(dto, profileCode);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción ManualCheckIn para un registro de tracking.
    /// </summary>
    /// <param name="dto">El parametro dto.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost("manual")]
    [Authorize(Roles = $"{RoleConstants.CoordinatorRole}, {RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> ManualCheckIn([FromBody] ManualCheckInDto dto)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.CoordinatorRole);
        var result = await _trackingService.ManualCheckInAsync(dto, requesterId, requesterRole);
        return CreatedAtAction(nameof(GetByCode), new { uvaCode = result.UvaCode }, result);
    }

    /// <summary>
    /// Procesa la acción ManualCheckOut para un registro de tracking.
    /// </summary>
    /// <param name="dto">El parametro dto.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost("manual/checkout")]
    [Authorize(Roles = $"{RoleConstants.CoordinatorRole}, {RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> ManualCheckOut([FromBody] ManualCheckOutDto dto)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.CoordinatorRole);
        var result = await _trackingService.ManualCheckOutAsync(dto, requesterId, requesterRole);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene los detalles de un registro de tracking específico.
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
        var result = await _trackingService.GetByCodeAsync(uvaCode);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción GetByEnrollment para un registro de tracking.
    /// </summary>
    /// <param name="enrollmentCode">El parametro enrollmentCode.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("by-enrollment/{enrollmentCode}")]
    public async Task<IActionResult> GetByEnrollment(string enrollmentCode)
    {
        var result = await _trackingService.GetByEnrollmentAsync(enrollmentCode);
        return Ok(result);
    }
}
