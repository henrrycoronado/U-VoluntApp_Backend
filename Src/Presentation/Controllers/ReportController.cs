namespace U_VoluntApp_Backend.Src.Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U_VoluntApp_Backend.Src.Application.DTOs;
using U_VoluntApp_Backend.Src.Application.Interfaces;
using U_VoluntApp_Backend.Src.Domain.Utils.Constants;
using U_VoluntApp_Backend.Src.Presentation.Helpers;

[ApiController]
[Route("api/v1/reports")]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// Procesa la acción GetScholarshipPerformance para un reporte.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("scholarships")]
    public async Task<IActionResult> GetScholarshipPerformance()
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.SuperUserRole);
        var result = await _reportService.GetScholarshipPerformanceAsync();
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción GetScholarshipPerformanceByType para un reporte.
    /// </summary>
    /// <param name="scholarshipType">El parametro scholarshipType.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("scholarships/by-type/{scholarshipType}")]
    public async Task<IActionResult> GetScholarshipPerformanceByType(string scholarshipType)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.SuperUserRole);
        var result = await _reportService.GetScholarshipPerformanceByTypeAsync(scholarshipType);
        return Ok(result);
    }

    /// <summary>
    /// Descarga o exporta un reporte.
    /// </summary>
    /// <param name="scholarshipType">El parametro scholarshipType.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("scholarships/pdf")]
    public async Task<IActionResult> DownloadScholarshipPdf([FromQuery] string? scholarshipType)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.SuperUserRole);
        var pdf = await _reportService.GenerateScholarshipPdfAsync(scholarshipType);

        var fileName = string.IsNullOrWhiteSpace(scholarshipType)
            ? "reporte_becas.pdf"
            : $"reporte_becas_{scholarshipType}.pdf";

        return File(pdf, "application/pdf", fileName);
    }

    /// <summary>
    /// Procesa la acción GetProgramAnalytics para un reporte.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("programs")]
    public async Task<IActionResult> GetProgramAnalytics()
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.SuperUserRole);
        var result = await _reportService.GetProgramAnalyticsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción GetProgramAnalyticsByCode para un reporte.
    /// </summary>
    /// <param name="programCode">El parametro programCode.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("programs/{programCode}")]
    public async Task<IActionResult> GetProgramAnalyticsByCode(string programCode)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.SuperUserRole);
        var result = await _reportService.GetProgramAnalyticsByCodeAsync(programCode);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción GetActivityAnalytics para un reporte.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("activities")]
    public async Task<IActionResult> GetActivityAnalytics()
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.SuperUserRole);
        var result = await _reportService.GetActivityAnalyticsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción GetActivityAnalyticsByProgram para un reporte.
    /// </summary>
    /// <param name="programCode">El parametro programCode.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("activities/by-program/{programCode}")]
    public async Task<IActionResult> GetActivityAnalyticsByProgram(string programCode)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.SuperUserRole);
        var result = await _reportService.GetActivityAnalyticsByProgramAsync(programCode);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción GetVolunteerHistory para un reporte.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("volunteers")]
    public async Task<IActionResult> GetVolunteerHistory()
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.SuperUserRole);
        var result = await _reportService.GetVolunteerHistoryAsync();
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción GetVolunteerHistoryByProfileCode para un reporte.
    /// </summary>
    /// <param name="profileCode">El parametro profileCode.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("volunteers/{profileCode}")]
    public async Task<IActionResult> GetVolunteerHistoryByProfileCode(string profileCode)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.SuperUserRole);
        var result = await _reportService.GetVolunteerHistoryByProfileCodeAsync(profileCode);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción RefreshAnalytics para un reporte.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAnalytics()
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.SuperUserRole);
        await _reportService.RefreshAnalyticsAsync();
        return Ok("Vistas materializadas actualizadas correctamente");
    }
}
