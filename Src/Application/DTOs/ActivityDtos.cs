namespace U_VoluntApp_Backend.Src.Application.DTOs;

public class CreateActivitySimpleDto
{
    public string ProgramCode { get; set; } = null!;

    public string ActivityTypeCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public double? LocationLatitude { get; set; }

    public double? LocationLongitude { get; set; }

    public bool RequiresEnrollment { get; set; }

    public bool RequiresApproval { get; set; }

    public int? Capacity { get; set; }

    public DateTime? EnrollmentDeadline { get; set; }
}

public class CreateActivityDto
{
    public string ProgramCode { get; set; } = null!;

    public string ActivityTypeCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? PhotoUrl { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public double? LocationLatitude { get; set; }

    public double? LocationLongitude { get; set; }

    public bool RequiresEnrollment { get; set; }

    public bool? CountsVolunteerHours { get; set; }

    public bool? IsMandatory { get; set; }

    public bool? IsPublicDropin { get; set; }

    public decimal? CostAmount { get; set; }

    public string? CostCurrency { get; set; }

    public CreateActivityRuleDto? Rule { get; set; }
}

public class CreateActivityRuleDto
{
    public int RegistrationRadiusMeters { get; set; }

    public DateTime? EnrollmentDeadline { get; set; }

    public bool RequiresApproval { get; set; }

    public int? TotalCapacity { get; set; }

    public List<CreateActivityGroupDto> Groups { get; set; } = [];
}

public class CreateActivityGroupDto
{
    public string Name { get; set; } = null!;

    public string? Details { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int? Capacity { get; set; }
}

public class UpdateActivityDto
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? PhotoUrl { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public double? LocationLatitude { get; set; }

    public double? LocationLongitude { get; set; }

    public string? ActivityTypeCode { get; set; }

    public bool? CountsVolunteerHours { get; set; }

    public bool? IsMandatory { get; set; }

    public bool? IsPublicDropin { get; set; }

    public decimal? CostAmount { get; set; }

    public string? CostCurrency { get; set; }
}

public class ActivityResponseDto
{
    public string UvaCode { get; set; } = null!;

    public string ProgramCode { get; set; } = null!;

    public string ProgramName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? PhotoUrl { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public double? LocationLatitude { get; set; }

    public double? LocationLongitude { get; set; }

    public bool RequiresEnrollment { get; set; }

    public string State { get; set; } = null!;

    public string StateCode { get; set; } = null!;

    public ActivityRuleResponseDto? Rule { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? LastModifiedByCode { get; set; }

    public DateTime? LastModifiedAt { get; set; }

    public string? LastModifiedByName { get; set; }
}

public class ActivityRuleResponseDto
{
    public string UvaCode { get; set; } = null!;

    public int RegistrationRadiusMeters { get; set; }

    public DateTime? EnrollmentDeadline { get; set; }

    public bool RequiresApproval { get; set; }

    public int? TotalCapacity { get; set; }

    public List<ActivityGroupResponseDto> Groups { get; set; } = [];
}

public class ActivityGroupResponseDto
{
    public string UvaCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Details { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int? Capacity { get; set; }
}

public class ChangeActivityStateDto
{
    public string StateCode { get; set; } = null!;
}
