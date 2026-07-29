namespace U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Profile;

using Microsoft.AspNetCore.Identity;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Activity;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Contract;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Enrollment;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.Tracking;
using U_VoluntApp_Core.Src.Infrastructure.Persistence.Models.VolProgram;

public partial class Profile
{
    public int Id { get; set; }

    public string UvaCode { get; set; } = null!;

    public string IdentityUserId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhotoUrl { get; set; }

    public string CareerCode { get; set; } = null!;

    public string? AddressLocation { get; set; }

    public string? Phone { get; set; }

    public decimal PersonalGoalHours { get; set; }

    public string StateCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual IdentityUser? IdentityUser { get; set; }

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual ICollection<ProgramCollaborator> ProgramCollaboratorAssignedByProfiles { get; set; } = new List<ProgramCollaborator>();

    public virtual ICollection<ProgramCollaborator> ProgramCollaboratorProfiles { get; set; } = new List<ProgramCollaborator>();

    public virtual ICollection<RoleRequest> RoleRequestRequesterProfiles { get; set; } = new List<RoleRequest>();

    public virtual ICollection<RoleRequest> RoleRequestResolvedByProfiles { get; set; } = new List<RoleRequest>();

    public virtual ICollection<TrackingLog> TrackingLogCheckInRegisteredBies { get; set; } = new List<TrackingLog>();

    public virtual ICollection<TrackingLog> TrackingLogCheckOutRegisteredBies { get; set; } = new List<TrackingLog>();

    public virtual ICollection<UserScholarship> UserScholarshipAssignedProfiles { get; set; } = new List<UserScholarship>();

    public virtual ICollection<UserScholarship> UserScholarshipEvaluatorProfiles { get; set; } = new List<UserScholarship>();

    public virtual ICollection<VolProgram> VolPrograms { get; set; } = new List<VolProgram>();
}
