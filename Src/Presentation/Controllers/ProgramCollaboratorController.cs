namespace U_VoluntApp_Backend.Src.Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;
using U_VoluntApp_Backend.Src.Presentation.Helpers;

[ApiController]
[Route("api/v1/collaborators")]
[Authorize]
public class ProgramCollaboratorController : ControllerBase
{
    private readonly IProgramCollaboratorService _collaboratorService;

    public ProgramCollaboratorController(IProgramCollaboratorService collaboratorService)
    {
        _collaboratorService = collaboratorService;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddProgramCollaboratorDto dto)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole);
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.AdminRole);
        var result = await _collaboratorService.AddAsync(dto, requesterId, requesterRole);
        return CreatedAtAction(nameof(GetByCode), new { uvaCode = result.UvaCode }, result);
    }

    [HttpGet("program/{programCode}")]
    public async Task<IActionResult> GetByProgramId(string programCode)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.VolunteerRole);
        var result = await _collaboratorService.GetByProgramIdAsync(programCode, requesterId, requesterRole);
        return Ok(ApiResponse<ProgramCollaboratorListDto>.Ok(result));
    }

    [HttpGet("{uvaCode}")]
    public async Task<IActionResult> GetByCode(string uvaCode)
    {
        var result = await _collaboratorService.GetByCodeAsync(uvaCode);
        if (result is null)
        {
            return NotFound(ApiResponse<object>.Fail("Colaborador no encontrado"));
        }

        return Ok(ApiResponse<ProgramCollaboratorResponseDto>.Ok(result));
    }

    [HttpPut("{uvaCode}")]
    public async Task<IActionResult> Update(string uvaCode, [FromBody] UpdateProgramCollaboratorDto dto)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.VolunteerRole);
        var result = await _collaboratorService.UpdateAsync(uvaCode, dto, requesterId, requesterRole);
        return Ok(ApiResponse<ProgramCollaboratorResponseDto>.Ok(result));
    }

    [HttpDelete("{uvaCode}")]
    public async Task<IActionResult> Delete(string uvaCode)
    {
        var (requesterId, requesterRole) = ControllerHelper.GetRequesterInfo(User, RoleConstants.VolunteerRole);
        await _collaboratorService.DeleteAsync(uvaCode, requesterId, requesterRole);
        return NoContent();
    }
}
