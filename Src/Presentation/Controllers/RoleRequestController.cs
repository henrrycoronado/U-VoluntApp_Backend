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
    public async Task<ActionResult<ApiResponse<RoleRequestResponseDto>>> RequestCoordinator(
        [FromBody] CreateRoleRequestDto dto)
    {
        var requesterCode = ControllerHelper.GetProfileId(User);
        var result = await _roleRequestService.RequestCoordinatorAsync(dto, requesterCode);
        return Ok(new ApiResponse<RoleRequestResponseDto> { Success = true, Data = result, Message = "Solicitud para Coordinador enviada exitosamente." });
    }

    [HttpPost("admin")]
    [Consumes("application/json")]
    [Authorize(Roles = "Coordinator,Admin,SuperUser")]
    public async Task<ActionResult<ApiResponse<RoleRequestResponseDto>>> RequestAdmin(
        [FromBody] CreateRoleRequestDto dto)
    {
        var requesterCode = ControllerHelper.GetProfileId(User);
        var result = await _roleRequestService.RequestAdminAsync(dto, requesterCode);
        return Ok(new ApiResponse<RoleRequestResponseDto> { Success = true, Data = result, Message = "Solicitud para Admin enviada exitosamente." });
    }

    [HttpGet("coordinator")]
    [Authorize(Roles = "Admin,SuperUser")]
    public async Task<ActionResult<ApiResponse<IEnumerable<RoleRequestResponseDto>>>> GetPendingCoordinatorRequests()
    {
        var result = await _roleRequestService.GetPendingCoordinatorRequestsAsync();
        return Ok(ApiResponse<IEnumerable<RoleRequestResponseDto>>.Ok(result));
    }

    [HttpGet("admin")]
    [Authorize(Roles = "SuperUser")]
    public async Task<ActionResult<ApiResponse<IEnumerable<RoleRequestResponseDto>>>> GetPendingAdminRequests()
    {
        var result = await _roleRequestService.GetPendingAdminRequestsAsync();
        return Ok(ApiResponse<IEnumerable<RoleRequestResponseDto>>.Ok(result));
    }

    [HttpPost("{uvaCode}/coordinator/approve")]
    [Consumes("application/json")]
    [Authorize(Roles = "Admin,SuperUser")]
    public async Task<ActionResult<ApiResponse<object>>> ApproveCoordinator(string uvaCode)
    {
        var adminCode = ControllerHelper.GetProfileId(User);
        await _roleRequestService.ApproveCoordinatorAsync(uvaCode, adminCode);
        return Ok(new ApiResponse<object> { Success = true, Message = "Solicitud de Coordinador aprobada." });
    }

    [Consumes("application/json")]
    [HttpPost("{uvaCode}/coordinator/reject")]
    [Authorize(Roles = "Admin,SuperUser")]
    public async Task<ActionResult<ApiResponse<object>>> RejectCoordinator(string uvaCode)
    {
        var adminCode = ControllerHelper.GetProfileId(User);
        await _roleRequestService.RejectCoordinatorAsync(uvaCode, adminCode);
        return Ok(new ApiResponse<object> { Success = true, Message = "Solicitud de Coordinador rechazada." });
    }

    [Consumes("application/json")]
    [HttpPost("{uvaCode}/admin/approve")]
    [Authorize(Roles = "SuperUser")]
    public async Task<ActionResult<ApiResponse<object>>> ApproveAdmin(string uvaCode)
    {
        var suCode = ControllerHelper.GetProfileId(User);
        await _roleRequestService.ApproveAdminAsync(uvaCode, suCode);
        return Ok(new ApiResponse<object> { Success = true, Message = "Solicitud de Admin aprobada." });
    }

    [Consumes("application/json")]
    [HttpPost("{uvaCode}/admin/reject")]
    [Authorize(Roles = "SuperUser")]
    public async Task<ActionResult<ApiResponse<object>>> RejectAdmin(string uvaCode)
    {
        var suCode = ControllerHelper.GetProfileId(User);
        await _roleRequestService.RejectAdminAsync(uvaCode, suCode);
        return Ok(new ApiResponse<object> { Success = true, Message = "Solicitud de Admin rechazada." });
    }
}
