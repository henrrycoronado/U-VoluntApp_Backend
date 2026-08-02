namespace U_VoluntApp_Core.Src.Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using U_VoluntApp_Core.Src.Application.DTOs;
using U_VoluntApp_Core.Src.Application.Interfaces;
using U_VoluntApp_Core.Src.Domain.Utils.Constants;
using U_VoluntApp_Core.Src.Presentation.Helpers;

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
    [Authorize(Roles = $"{RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> GetScholarshipPerformance()
    {
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
    [Authorize(Roles = $"{RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> GetScholarshipPerformanceByType(string scholarshipType)
    {
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
    [Authorize(Roles = $"{RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> DownloadScholarshipPdf([FromQuery] string? scholarshipType)
    {
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
    [Authorize(Roles = $"{RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> GetVolunteerHistory()
    {
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
    [Authorize(Roles = $"{RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> GetVolunteerHistoryByProfileCode(string profileCode)
    {
        var result = await _reportService.GetVolunteerHistoryByProfileCodeAsync(profileCode);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción GetVolunteerHistoryByProfileCode para un reporte.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("volunteers/me")]
    public async Task<IActionResult> GetVolunteerHistoryMe()
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        var result = await _reportService.GetVolunteerHistoryByProfileCodeAsync(profileCode);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción GetHomeSummaryMe para obtener un resumen del perfil actual.
    /// </summary>
    /// <param name="year">Año opcional (defecto año actual).</param>
    /// <param name="month">Mes opcional (defecto mes actual).</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("volunteers/me/home-summary")]
    public async Task<IActionResult> GetHomeSummaryMe([FromQuery] int? year, [FromQuery] int? month)
    {
        var profileCode = ControllerHelper.GetProfileId(User);
        var result = await _reportService.GetHomeSummaryAsync(profileCode, year, month);
        return Ok(result);
    }

    /// <summary>
    /// Procesa la acción GetAdminHomeSummary para el panel de control del administrador.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("admin/home-summary")]
    [Authorize(Roles = $"{RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> GetAdminHomeSummary()
    {
        var result = await _reportService.GetAdminHomeSummaryAsync();
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
    [Authorize(Roles = $"{RoleConstants.AdminRole}, {RoleConstants.SuperUserRole}")]
    public async Task<IActionResult> RefreshAnalytics()
    {
        await _reportService.RefreshAnalyticsAsync();
        return Ok("Vistas materializadas actualizadas correctamente");
    }
}
