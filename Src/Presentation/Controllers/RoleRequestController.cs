namespace U_VoluntApp_Core.Src.Presentation.Controllers;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U_VoluntApp_Core.Src.Application.DTOs;
using U_VoluntApp_Core.Src.Application.Interfaces;
using U_VoluntApp_Core.Src.Domain.Utils.Constants;
using U_VoluntApp_Core.Src.Presentation.Helpers;

[ApiController]
[Route("api/v1/roles/requests")]
[Authorize]
public class RoleRequestController : ControllerBase
{
    private readonly IRoleRequestService _roleRequestService;

    public RoleRequestController(IRoleRequestService roleRequestService)
    {
        _roleRequestService = roleRequestService;
    }

    /// <summary>
    /// Procesa la acción RequestCoordinator para una solicitud de rol.
    /// </summary>
    /// <param name="dto">El parametro dto.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(typeof(RoleRequestResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost("coordinator")]
    [Consumes("application/json")]
    [Authorize(Roles = RoleConstants.VolunteerRole)]
    public async Task<ActionResult<RoleRequestResponseDto>> RequestCoordinator(
            [FromBody] CreateRoleRequestDto dto)
    {
        var requesterCode = ControllerHelper.GetProfileId(User);
        var result = await _roleRequestService.RequestCoordinatorAsync(dto, requesterCode);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción RequestAdmin para una solicitud de rol.
    /// </summary>
    /// <param name="dto">El parametro dto.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(typeof(RoleRequestResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost("admin")]
    [Consumes("application/json")]
    [Authorize(Roles = RoleConstants.CoordinatorRole)]
    public async Task<ActionResult<RoleRequestResponseDto>> RequestAdmin(
            [FromBody] CreateRoleRequestDto dto)
    {
        var requesterCode = ControllerHelper.GetProfileId(User);
        var result = await _roleRequestService.RequestAdminAsync(dto, requesterCode);
        return Ok(result);
    }

    [HttpGet("coordinator")]
    [Authorize(Roles = $"{RoleConstants.AdminRole},{RoleConstants.SuperUserRole}")]
    public async Task<ActionResult<IEnumerable<RoleRequestResponseDto>>> GetPendingCoordinatorRequests()
    {
        var result = await _roleRequestService.GetPendingCoordinatorRequestsAsync();
        return Ok(result);
    }

    [HttpGet("admin")]
    [Authorize(Roles = RoleConstants.SuperUserRole)]
    public async Task<ActionResult<IEnumerable<RoleRequestResponseDto>>> GetPendingAdminRequests()
    {
        var result = await _roleRequestService.GetPendingAdminRequestsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Aprueba una solicitud de rol.
    /// </summary>
    /// <param name="uvaCode">El parametro uvaCode.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost("{uvaCode}/coordinator/approve")]
    [Consumes("application/json")]
    [Authorize(Roles = $"{RoleConstants.AdminRole},{RoleConstants.SuperUserRole}")]
    public async Task<ActionResult<string>> ApproveCoordinator(string uvaCode)
    {
        var adminCode = ControllerHelper.GetProfileId(User);
        await _roleRequestService.ApproveCoordinatorAsync(uvaCode, adminCode);
        return Ok("Solicitud de Coordinador aprobada.");
    }

    /// <summary>
    /// Rechaza una solicitud de rol.
    /// </summary>
    /// <param name="uvaCode">El parametro uvaCode.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost("{uvaCode}/coordinator/reject")]
    [Consumes("application/json")]
    [Authorize(Roles = $"{RoleConstants.AdminRole},{RoleConstants.SuperUserRole}")]
    public async Task<ActionResult<string>> RejectCoordinator(string uvaCode)
    {
        var adminCode = ControllerHelper.GetProfileId(User);
        await _roleRequestService.RejectCoordinatorAsync(uvaCode, adminCode);
        return Ok("Solicitud de Coordinador rechazada.");
    }

    /// <summary>
    /// Aprueba una solicitud de rol.
    /// </summary>
    /// <param name="uvaCode">El parametro uvaCode.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost("{uvaCode}/admin/approve")]
    [Consumes("application/json")]
    [Authorize(Roles = RoleConstants.SuperUserRole)]
    public async Task<ActionResult<string>> ApproveAdmin(string uvaCode)
    {
        var suCode = ControllerHelper.GetProfileId(User);
        await _roleRequestService.ApproveAdminAsync(uvaCode, suCode);
        return Ok("Solicitud de Admin aprobada.");
    }

    /// <summary>
    /// Rechaza una solicitud de rol.
    /// </summary>
    /// <param name="uvaCode">El parametro uvaCode.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost("{uvaCode}/admin/reject")]
    [Consumes("application/json")]
    [Authorize(Roles = RoleConstants.SuperUserRole)]
    public async Task<ActionResult<string>> RejectAdmin(string uvaCode)
    {
        var suCode = ControllerHelper.GetProfileId(User);
        await _roleRequestService.RejectAdminAsync(uvaCode, suCode);
        return Ok("Solicitud de Admin rechazada.");
    }
}
