namespace U_VoluntApp_Core.Src.Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U_VoluntApp_Core.Src.Application.DTOs;
using U_VoluntApp_Core.Src.Application.Interfaces;
using U_VoluntApp_Core.Src.Domain.Utils.Constants;
using U_VoluntApp_Core.Src.Presentation.Helpers;

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

    /// <summary>
    /// Crea una beca nuevo.
    /// </summary>
    /// <param name="dto">El parametro dto.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateScholarshipRequestDto dto)
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        var result = await _scholarshipService.RequestAsync(dto, profileCode);
        return CreatedAtAction(nameof(GetByCode), new { uvaCode = result.UvaCode }, result);
    }

    /// <summary>
    /// Procesa la acción Assign para una beca.
    /// </summary>
    /// <param name="dto">El parametro dto.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost("assign")]
    [Authorize(Roles = $"{RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> Assign([FromBody] CreateScholarshipForProfileDto dto)
    {
        var evaluatorCode = ControllerHelper.GetProfileId(User);
        var result = await _scholarshipService.AssignApprovedAsync(dto, evaluatorCode);
        return CreatedAtAction(nameof(GetByCode), new { uvaCode = result.UvaCode }, result);
    }

    /// <summary>
    /// Obtiene los detalles de una beca específico.
    /// </summary>
    /// <param name="uvaCode">El parametro uvaCode.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("{uvaCode}")]
    public async Task<IActionResult> GetByCode(string uvaCode)
    {
        var result = await _scholarshipService.GetByCodeAsync(uvaCode);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción GetMine para una beca.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("mine")]
    public async Task<IActionResult> GetMine()
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        var result = await _scholarshipService.GetMyAsync(profileCode);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción GetByProfile para una beca.
    /// </summary>
    /// <param name="profileCode">El parametro profileCode.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("by-profile/{profileCode}")]
    [Authorize(Roles = $"{RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> GetByProfile(string profileCode)
    {
        var result = await _scholarshipService.GetByProfileCodeAsync(profileCode);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción Review para una beca.
    /// </summary>
    /// <param name="uvaCode">El parametro uvaCode.</param>
    /// <param name="dto">El parametro dto.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPatch("{uvaCode}/review")]
    [Authorize(Roles = $"{RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> Review(string uvaCode, [FromBody] ReviewScholarshipDto dto)
    {
        var evaluatorCode = ControllerHelper.GetProfileId(User);
        var result = await _scholarshipService.ReviewAsync(uvaCode, dto, evaluatorCode);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción Complete para una beca.
    /// </summary>
    /// <param name="uvaCode">El parametro uvaCode.</param>
    /// <param name="dto">El parametro dto.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPatch("{uvaCode}/complete")]
    [Authorize(Roles = $"{RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> Complete(string uvaCode, [FromBody] CompleteScholarshipDto dto)
    {
        var evaluatorCode = ControllerHelper.GetProfileId(User);
        var result = await _scholarshipService.CompleteAsync(uvaCode, dto, evaluatorCode);
        return Ok(result);
    }
}
