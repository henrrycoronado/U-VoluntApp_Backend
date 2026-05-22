namespace U_VoluntApp_Backend.Src.Application.DTOs;

using Microsoft.AspNetCore.Http;

public class CheckInDto
{
    public string EnrollmentCode { get; set; } = null!;

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public IFormFile? Evidence { get; set; }
}

public class CheckOutDto
{
    public string TrackingLogCode { get; set; } = null!;

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public IFormFile? Evidence { get; set; }
}

public class ManualCheckInDto
{
    public string EnrollmentCode { get; set; } = null!;

    public DateTime EntryTime { get; set; }

    public DateTime? ExitTime { get; set; }

    public decimal CalculatedHours { get; set; }

    public string? Observations { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }
}

public class ManualCheckOutDto
{
    public string TrackingLogCode { get; set; } = null!;

    public DateTime? ExitTime { get; set; }

    public string? Observations { get; set; }
}

public class TrackingLogResponseDto
{
    public string UvaCode { get; set; } = null!;

    public string EnrollmentCode { get; set; } = null!;

    public string VolunteerName { get; set; } = null!;

    public string ActivityName { get; set; } = null!;

    public DateTime? EntryTime { get; set; }

    public DateTime? ExitTime { get; set; }

    public decimal CalculatedHours { get; set; }

    public string? Observations { get; set; }

    public string TypeCode { get; set; } = null!;

    public string StateCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? CheckInRegisteredByCode { get; set; }

    public string? CheckInRegisteredByName { get; set; }

    public string? CheckOutRegisteredByCode { get; set; }

    public string? CheckOutRegisteredByName { get; set; }
}
