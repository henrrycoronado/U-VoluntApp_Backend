namespace U_VoluntApp_Backend.Src.Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;
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

    [HttpPost]
    public async Task<IActionResult> Enroll([FromBody] CreateEnrollmentDto dto)
    {
        var profileCode = ControllerHelper.GetProfileId(User); // Assuming this returns uva_code
        var result = await _enrollmentService.EnrollAsync(dto, profileCode);
        return CreatedAtAction(nameof(GetByCode), new { uvaCode = result.UvaCode }, result);
    }

    [HttpGet("{uvaCode}")]
    public async Task<IActionResult> GetByCode(string uvaCode)
    {
        var result = await _enrollmentService.GetByCodeAsync(uvaCode);
        return Ok(result);
    }

    [HttpGet("by-activity/{activityCode}")]
    public async Task<IActionResult> GetByActivity(string activityCode)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.VolunteerRole);
        var result = await _enrollmentService.GetByActivityAsync(activityCode, requesterId, requesterRole);
        return Ok(result);
    }

    [HttpGet("mine")]
    public async Task<IActionResult> GetMyEnrollments()
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        var result = await _enrollmentService.GetMyEnrollmentsAsync(profileCode);
        return Ok(result);
    }

    [HttpPatch("{uvaCode}/review")]
    public async Task<IActionResult> Review(string uvaCode, [FromBody] ReviewEnrollmentDto dto)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.CoordinatorRole);
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.CoordinatorRole);
        await _enrollmentService.ReviewAsync(uvaCode, dto, requesterId, requesterRole);
        return NoContent();
    }

    [HttpPatch("{uvaCode}/cancel")]
    public async Task<IActionResult> Cancel(string uvaCode)
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        await _enrollmentService.CancelAsync(uvaCode, profileCode);
        return NoContent();
    }
}
