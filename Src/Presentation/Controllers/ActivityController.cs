namespace U_VoluntApp_Backend.Src.Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;
using U_VoluntApp_Backend.Src.Presentation.Helpers;

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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateActivityDto dto)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.CoordinatorRole);
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.AdminRole);
        var result = await _activityService.CreateAsync(dto, requesterId, requesterRole);
        return CreatedAtAction(nameof(GetByCode), new { uvaCode = result.UvaCode }, result);
    }

    [HttpPost("simple")]
    public async Task<IActionResult> CreateSimple([FromBody] CreateActivitySimpleDto dto)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.CoordinatorRole);
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.AdminRole);
        var result = await _activityService.CreateSimpleAsync(dto, requesterId, requesterRole);
        return CreatedAtAction(nameof(GetByCode), new { uvaCode = result.UvaCode }, result);
    }

    [HttpGet("{uvaCode}")]
    public async Task<IActionResult> GetByCode(string uvaCode)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.VolunteerRole);
        var result = await _activityService.GetByCodeAsync(uvaCode, requesterId, requesterRole);
        return Ok(result);
    }

    [HttpGet("by-program/{programCode}")]
    public async Task<IActionResult> GetByProgram(string programCode)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.CoordinatorRole, RoleConstants.VolunteerRole);
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.AdminRole);
        var result = await _activityService.GetByProgramAsync(programCode, requesterId, requesterRole);
        return Ok(result);
    }

    [HttpPut("{uvaCode}")]
    public async Task<IActionResult> Update(string uvaCode, [FromBody] UpdateActivityDto dto)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.CoordinatorRole);
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.AdminRole);
        var result = await _activityService.UpdateAsync(uvaCode, dto, requesterId, requesterRole);
        return Ok(result);
    }

    [HttpPatch("{uvaCode}/state")]
    public async Task<IActionResult> ChangeState(string uvaCode, [FromBody] ChangeActivityStateDto dto)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.CoordinatorRole);
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.AdminRole);
        await _activityService.ChangeStateAsync(uvaCode, dto, requesterId, requesterRole);
        return NoContent();
    }

    [HttpDelete("{uvaCode}")]
    public async Task<IActionResult> Delete(string uvaCode)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.CoordinatorRole);
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.AdminRole);
        await _activityService.DeleteAsync(uvaCode, requesterId, requesterRole);
        return NoContent();
    }
}
