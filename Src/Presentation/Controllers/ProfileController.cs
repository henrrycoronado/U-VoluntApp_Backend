namespace U_VoluntApp_Backend.Src.Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Presentation.Helpers;

[ApiController]
[Route("api/v1/profiles")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        var result = await _profileService.GetByCodeAsync(profileCode);
        return Ok(ApiResponse<ProfileResponseDto>.Ok(result));
    }

    [HttpGet("{uvaCode}")]
    public async Task<IActionResult> GetByCode(string uvaCode)
    {
        var result = await _profileService.GetByCodeAsync(uvaCode);
        return Ok(ApiResponse<ProfileResponseDto>.Ok(result));
    }

    [HttpPut("me")]
    public async Task<IActionResult> Update([FromBody] UpdateProfileDto dto)
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        var result = await _profileService.UpdateAsync(profileCode, dto);
        return Ok(ApiResponse<ProfileResponseDto>.Ok(result));
    }

    [HttpPatch("me/photo")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdatePhoto(IFormFile photo)
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        var result = await _profileService.UpdatePhotoAsync(profileCode, photo);
        return Ok(ApiResponse<ProfileResponseDto>.Ok(result));
    }

    [HttpDelete("me")]
    public async Task<IActionResult> Delete()
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        await _profileService.DeleteAsync(profileCode);
        return NoContent();
    }
}
