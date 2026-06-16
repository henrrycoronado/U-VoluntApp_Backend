namespace U_VoluntApp_Backend.Src.Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("scholarships")]
    public async Task<IActionResult> GetScholarshipPerformance()
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.SuperUserRole);
        var result = await _reportService.GetScholarshipPerformanceAsync();
        return Ok(ApiResponse<List<ScholarshipPerformanceDto>>.Ok(result));
    }

    [HttpGet("scholarships/by-type/{scholarshipType}")]
    public async Task<IActionResult> GetScholarshipPerformanceByType(string scholarshipType)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.SuperUserRole);
        var result = await _reportService.GetScholarshipPerformanceByTypeAsync(scholarshipType);
        return Ok(ApiResponse<List<ScholarshipPerformanceDto>>.Ok(result));
    }

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

    [HttpGet("programs")]
    public async Task<IActionResult> GetProgramAnalytics()
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.SuperUserRole);
        var result = await _reportService.GetProgramAnalyticsAsync();
        return Ok(ApiResponse<List<ProgramAnalyticsDto>>.Ok(result));
    }

    [HttpGet("programs/{programCode}")]
    public async Task<IActionResult> GetProgramAnalyticsByCode(string programCode)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.SuperUserRole);
        var result = await _reportService.GetProgramAnalyticsByCodeAsync(programCode);
        return Ok(ApiResponse<ProgramAnalyticsDto>.Ok(result));
    }

    [HttpGet("activities")]
    public async Task<IActionResult> GetActivityAnalytics()
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.SuperUserRole);
        var result = await _reportService.GetActivityAnalyticsAsync();
        return Ok(ApiResponse<List<ActivityAnalyticsDto>>.Ok(result));
    }

    [HttpGet("activities/by-program/{programCode}")]
    public async Task<IActionResult> GetActivityAnalyticsByProgram(string programCode)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.SuperUserRole);
        var result = await _reportService.GetActivityAnalyticsByProgramAsync(programCode);
        return Ok(ApiResponse<List<ActivityAnalyticsDto>>.Ok(result));
    }

    [HttpGet("volunteers")]
    public async Task<IActionResult> GetVolunteerHistory()
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.SuperUserRole);
        var result = await _reportService.GetVolunteerHistoryAsync();
        return Ok(ApiResponse<List<VolunteerHistoryDto>>.Ok(result));
    }

    [HttpGet("volunteers/{profileCode}")]
    public async Task<IActionResult> GetVolunteerHistoryByProfileCode(string profileCode)
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.SuperUserRole);
        var result = await _reportService.GetVolunteerHistoryByProfileCodeAsync(profileCode);
        return Ok(ApiResponse<VolunteerHistoryDto>.Ok(result));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAnalytics()
    {
        ControllerHelper.EnsureRole(User, RoleConstants.AdminRole, RoleConstants.SuperUserRole);
        await _reportService.RefreshAnalyticsAsync();
        return Ok(ApiResponse<string>.Ok("Vistas materializadas actualizadas correctamente"));
    }
}
