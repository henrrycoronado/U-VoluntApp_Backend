namespace U_VoluntApp_Backend.Src.Presentation.Controllers;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Presentation.Helpers;

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

    [HttpPost("coordinator")]
    [Consumes("application/json")]
    public async Task<ActionResult<RoleRequestResponseDto>> RequestCoordinator(
        [FromBody] CreateRoleRequestDto dto)
    {
        var requesterCode = ControllerHelper.GetProfileId(User);
        var result = await _roleRequestService.RequestCoordinatorAsync(dto, requesterCode);
        return Ok(result);
    }

    [HttpPost("admin")]
    [Consumes("application/json")]
    [Authorize(Roles = "Coordinator,Admin,SuperUser")]
    public async Task<ActionResult<RoleRequestResponseDto>> RequestAdmin(
        [FromBody] CreateRoleRequestDto dto)
    {
        var requesterCode = ControllerHelper.GetProfileId(User);
        var result = await _roleRequestService.RequestAdminAsync(dto, requesterCode);
        return Ok(result);
    }

    [HttpGet("coordinator")]
    [Authorize(Roles = "Admin,SuperUser")]
    public async Task<ActionResult<IEnumerable<RoleRequestResponseDto>>> GetPendingCoordinatorRequests()
    {
        var result = await _roleRequestService.GetPendingCoordinatorRequestsAsync();
        return Ok(result);
    }

    [HttpGet("admin")]
    [Authorize(Roles = "SuperUser")]
    public async Task<ActionResult<IEnumerable<RoleRequestResponseDto>>> GetPendingAdminRequests()
    {
        var result = await _roleRequestService.GetPendingAdminRequestsAsync();
        return Ok(result);
    }

    [HttpPost("{uvaCode}/coordinator/approve")]
    [Consumes("application/json")]
    [Authorize(Roles = "Admin,SuperUser")]
    public async Task<ActionResult<string>> ApproveCoordinator(string uvaCode)
    {
        var adminCode = ControllerHelper.GetProfileId(User);
        await _roleRequestService.ApproveCoordinatorAsync(uvaCode, adminCode);
        return Ok("Solicitud de Coordinador aprobada.");
    }

    [Consumes("application/json")]
    [HttpPost("{uvaCode}/coordinator/reject")]
    [Authorize(Roles = "Admin,SuperUser")]
    public async Task<ActionResult<string>> RejectCoordinator(string uvaCode)
    {
        var adminCode = ControllerHelper.GetProfileId(User);
        await _roleRequestService.RejectCoordinatorAsync(uvaCode, adminCode);
        return Ok("Solicitud de Coordinador rechazada.");
    }

    [Consumes("application/json")]
    [HttpPost("{uvaCode}/admin/approve")]
    [Authorize(Roles = "SuperUser")]
    public async Task<ActionResult<string>> ApproveAdmin(string uvaCode)
    {
        var suCode = ControllerHelper.GetProfileId(User);
        await _roleRequestService.ApproveAdminAsync(uvaCode, suCode);
        return Ok("Solicitud de Admin aprobada.");
    }

    [Consumes("application/json")]
    [HttpPost("{uvaCode}/admin/reject")]
    [Authorize(Roles = "SuperUser")]
    public async Task<ActionResult<string>> RejectAdmin(string uvaCode)
    {
        var suCode = ControllerHelper.GetProfileId(User);
        await _roleRequestService.RejectAdminAsync(uvaCode, suCode);
        return Ok("Solicitud de Admin rechazada.");
    }
}
