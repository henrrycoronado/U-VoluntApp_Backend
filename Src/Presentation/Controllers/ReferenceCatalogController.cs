namespace U_VoluntApp_Backend.Src.Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;
using U_VoluntApp_Backend.Src.Presentation.Helpers;

[ApiController]
[Route("api/v1/reference-catalog")]
public class ReferenceCatalogController : ControllerBase
{
    private readonly IReferenceCatalogService _referenceCatalogService;

    public ReferenceCatalogController(IReferenceCatalogService referenceCatalogService)
    {
        _referenceCatalogService = referenceCatalogService;
    }

    /// <summary>
    /// Procesa la acción GetStates para un catálogo de referencia.
    /// </summary>
    /// <param name="stateGroup">El parametro stateGroup.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("states/{stateGroup}")]
    public async Task<IActionResult> GetStates(string stateGroup)
    {
        var result = await _referenceCatalogService.GetStatesAsync(stateGroup);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción UpdateStateName para un catálogo de referencia.
    /// </summary>
    /// <param name="stateGroup">El parametro stateGroup.</param>
    /// <param name="stateCode">El parametro stateCode.</param>
    /// <param name="dto">El parametro dto.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPatch("states/{stateGroup}/{stateCode}")]
    [Authorize(Roles = RoleConstants.SuperUserRole)]
    public async Task<IActionResult> UpdateStateName(string stateGroup, string stateCode, [FromBody] UpdateReferenceStateNameDto dto)
    {
        var result = await _referenceCatalogService.UpdateStateNameAsync(stateGroup, stateCode, dto.Name);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción GetTypes para un catálogo de referencia.
    /// </summary>
    /// <param name="typeGroup">El parametro typeGroup.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("types/{typeGroup}")]
    public async Task<IActionResult> GetTypes(string typeGroup)
    {
        var result = await _referenceCatalogService.GetTypesAsync(typeGroup);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción CreateType para un catálogo de referencia.
    /// </summary>
    /// <param name="typeGroup">El parametro typeGroup.</param>
    /// <param name="dto">El parametro dto.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost("types/{typeGroup}")]
    [Authorize(Roles = RoleConstants.SuperUserRole)]
    public async Task<IActionResult> CreateType(string typeGroup, [FromBody] CreateReferenceTypeDto dto)
    {
        var result = await _referenceCatalogService.CreateTypeAsync(typeGroup, dto);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción UpdateType para un catálogo de referencia.
    /// </summary>
    /// <param name="typeGroup">El parametro typeGroup.</param>
    /// <param name="typeCode">El parametro typeCode.</param>
    /// <param name="dto">El parametro dto.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPatch("types/{typeGroup}/{typeCode}")]
    [Authorize(Roles = RoleConstants.SuperUserRole)]
    public async Task<IActionResult> UpdateType(string typeGroup, string typeCode, [FromBody] UpdateReferenceTypeDto dto)
    {
        var result = await _referenceCatalogService.UpdateTypeAsync(typeGroup, typeCode, dto);
        return Ok(result);
    }
}
