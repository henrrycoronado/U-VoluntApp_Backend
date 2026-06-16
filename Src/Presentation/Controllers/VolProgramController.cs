namespace U_VoluntApp_Backend.Src.Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;
using U_VoluntApp_Backend.Src.Presentation.Helpers;

[ApiController]
[Route("api/v1/programs")]
[Authorize]
public class VolProgramController : ControllerBase
{
    private readonly IVolProgramService _volProgramService;

    public VolProgramController(IVolProgramService volProgramService)
    {
        _volProgramService = volProgramService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVolProgramDto dto)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole);
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.AdminRole);
        var result = await _volProgramService.CreateAsync(dto, requesterId, requesterRole);
        return CreatedAtAction(nameof(GetByCode), new { uvaCode = result.UvaCode }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.CoordinatorRole);
        var result = await _volProgramService.GetAllAsync(requesterId, requesterRole);
        return Ok(result);
    }

    [HttpGet("{uvaCode}")]
    public async Task<IActionResult> GetByCode(string uvaCode)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.CoordinatorRole);
        var result = await _volProgramService.GetByCodeAsync(uvaCode, requesterId, requesterRole);
        return Ok(result);
    }

    [HttpPut("{uvaCode}")]
    public async Task<IActionResult> Update(string uvaCode, [FromBody] UpdateVolProgramDto dto)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole);
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.AdminRole);
        var result = await _volProgramService.UpdateAsync(uvaCode, dto, requesterId, requesterRole);
        return Ok(result);
    }

    [HttpPatch("{uvaCode}/state")]
    public async Task<IActionResult> ChangeState(string uvaCode, [FromBody] ChangeVolProgramStateDto dto)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.SuperUserRole);
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.SuperUserRole);
        await _volProgramService.ChangeStateAsync(uvaCode, dto, requesterId, requesterRole);
        return NoContent();
    }

    [HttpDelete("{uvaCode}")]
    public async Task<IActionResult> Delete(string uvaCode)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole);
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.AdminRole);
        await _volProgramService.DeleteAsync(uvaCode, requesterId, requesterRole);
        return NoContent();
    }
}
