namespace U_VoluntApp_Backend.Src.Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Presentation.Helpers;

[ApiController]
[Route("api/v1/scholarships")]
[Authorize]
public class ScholarshipController : ControllerBase
{
    private readonly IUserScholarshipService _scholarshipService;

    public ScholarshipController(IUserScholarshipService scholarshipService)
    {
        _scholarshipService = scholarshipService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateScholarshipRequestDto dto)
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        var result = await _scholarshipService.RequestAsync(dto, profileCode);
        return CreatedAtAction(nameof(GetByCode), new { uvaCode = result.UvaCode }, result);
    }

    [HttpPost("assign")]
    public async Task<IActionResult> Assign([FromBody] CreateScholarshipForProfileDto dto)
    {
        ControllerHelper.EnsureRole(User, "Admin");
        var evaluatorCode = ControllerHelper.GetProfileId(User);
        var result = await _scholarshipService.AssignApprovedAsync(dto, evaluatorCode);
        return CreatedAtAction(nameof(GetByCode), new { uvaCode = result.UvaCode }, result);
    }

    [HttpGet("{uvaCode}")]
    public async Task<IActionResult> GetByCode(string uvaCode)
    {
        var result = await _scholarshipService.GetByCodeAsync(uvaCode);
        return Ok(result);
    }

    [HttpGet("mine")]
    public async Task<IActionResult> GetMine()
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        var result = await _scholarshipService.GetMyAsync(profileCode);
        return Ok(result);
    }

    [HttpGet("by-profile/{profileCode}")]
    public async Task<IActionResult> GetByProfile(string profileCode)
    {
        ControllerHelper.EnsureRole(User, "Admin");
        var result = await _scholarshipService.GetByProfileCodeAsync(profileCode);
        return Ok(result);
    }

    [HttpPatch("{uvaCode}/review")]
    public async Task<IActionResult> Review(string uvaCode, [FromBody] ReviewScholarshipDto dto)
    {
        ControllerHelper.EnsureRole(User, "Admin");
        var evaluatorCode = ControllerHelper.GetProfileId(User);
        var result = await _scholarshipService.ReviewAsync(uvaCode, dto, evaluatorCode);
        return Ok(result);
    }

    [HttpPatch("{uvaCode}/complete")]
    public async Task<IActionResult> Complete(string uvaCode, [FromBody] CompleteScholarshipDto dto)
    {
        ControllerHelper.EnsureRole(User, "Admin");
        var evaluatorCode = ControllerHelper.GetProfileId(User);
        var result = await _scholarshipService.CompleteAsync(uvaCode, dto, evaluatorCode);
        return Ok(result);
    }
}
