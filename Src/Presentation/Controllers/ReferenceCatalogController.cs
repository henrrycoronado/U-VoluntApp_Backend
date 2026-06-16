namespace U_VoluntApp_Backend.Src.Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;
using U_VoluntApp_Backend.Src.Presentation.Helpers;

[ApiController]
[Route("api/v1/reference-catalog")]
[Authorize]
public class ReferenceCatalogController : ControllerBase
{
    private readonly IReferenceCatalogService _referenceCatalogService;

    public ReferenceCatalogController(IReferenceCatalogService referenceCatalogService)
    {
        _referenceCatalogService = referenceCatalogService;
    }

    [HttpGet("states/{stateGroup}")]
    public async Task<IActionResult> GetStates(string stateGroup)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.SuperUserRole);
        var result = await _referenceCatalogService.GetStatesAsync(stateGroup);
        return Ok(ApiResponse<List<ReferenceStateDto>>.Ok(result));
    }

    [HttpPatch("states/{stateGroup}/{stateCode}")]
    public async Task<IActionResult> UpdateStateName(string stateGroup, string stateCode, [FromBody] UpdateReferenceStateNameDto dto)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.SuperUserRole);
        var result = await _referenceCatalogService.UpdateStateNameAsync(stateGroup, stateCode, dto.Name);
        return Ok(ApiResponse<ReferenceStateDto>.Ok(result));
    }

    [HttpGet("types/{typeGroup}")]
    public async Task<IActionResult> GetTypes(string typeGroup)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.SuperUserRole);
        var result = await _referenceCatalogService.GetTypesAsync(typeGroup);
        return Ok(ApiResponse<List<ReferenceTypeDto>>.Ok(result));
    }

    [HttpPost("types/{typeGroup}")]
    public async Task<IActionResult> CreateType(string typeGroup, [FromBody] CreateReferenceTypeDto dto)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.SuperUserRole);
        var result = await _referenceCatalogService.CreateTypeAsync(typeGroup, dto);
        return Ok(ApiResponse<ReferenceTypeDto>.Ok(result));
    }

    [HttpPatch("types/{typeGroup}/{typeCode}")]
    public async Task<IActionResult> UpdateType(string typeGroup, string typeCode, [FromBody] UpdateReferenceTypeDto dto)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.SuperUserRole);
        var result = await _referenceCatalogService.UpdateTypeAsync(typeGroup, typeCode, dto);
        return Ok(ApiResponse<ReferenceTypeDto>.Ok(result));
    }
}
