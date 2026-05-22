namespace U_VoluntApp_Backend.Src.Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;
using U_VoluntApp_Backend.Src.Presentation.Helpers;

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

    [HttpPost("checkin")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CheckIn([FromForm] CheckInDto dto)
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        var result = await _trackingService.CheckInAsync(dto, profileCode);
        return CreatedAtAction(nameof(GetByCode), new { uvaCode = result.UvaCode }, result);
    }

    [HttpPost("checkout")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CheckOut([FromForm] CheckOutDto dto)
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        var result = await _trackingService.CheckOutAsync(dto, profileCode);
        return Ok(result);
    }

    [HttpPost("manual")]
    public async Task<IActionResult> ManualCheckIn([FromBody] ManualCheckInDto dto)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.CoordinatorRole);
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, "Coordinator");
        var result = await _trackingService.ManualCheckInAsync(dto, requesterId, requesterRole);
        return CreatedAtAction(nameof(GetByCode), new { uvaCode = result.UvaCode }, result);
    }

    [HttpPost("manual/checkout")]
    public async Task<IActionResult> ManualCheckOut([FromBody] ManualCheckOutDto dto)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.CoordinatorRole);
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, "Coordinator");
        var result = await _trackingService.ManualCheckOutAsync(dto, requesterId, requesterRole);
        return Ok(result);
    }

    [HttpGet("{uvaCode}")]
    public async Task<IActionResult> GetByCode(string uvaCode)
    {
        var result = await _trackingService.GetByCodeAsync(uvaCode);
        return Ok(result);
    }

    [HttpGet("by-enrollment/{enrollmentCode}")]
    public async Task<IActionResult> GetByEnrollment(string enrollmentCode)
    {
        var result = await _trackingService.GetByEnrollmentAsync(enrollmentCode);
        return Ok(result);
    }
}
