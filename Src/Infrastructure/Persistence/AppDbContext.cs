namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Activity;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Auth;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Contract;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Enrollment;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Profile;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.States;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Tracking;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Types;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.VolProgram;

public partial class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<ActivityGroup> ActivityGroups { get; set; }

    public virtual DbSet<ActivityRecurrenceDetail> ActivityRecurrenceDetails { get; set; }

    public virtual DbSet<ActivityRecurrencePattern> ActivityRecurrencePatterns { get; set; }

    public virtual DbSet<ActivityRule> ActivityRules { get; set; }

    public virtual DbSet<ActivityState> ActivityStates { get; set; }

    public virtual DbSet<ActivityType> ActivityTypes { get; set; }

    public virtual DbSet<CareerType> CareerTypes { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<ScholarshipType> ScholarshipTypes { get; set; }

    public virtual DbSet<ContractState> ContractStates { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<EnrollmentState> EnrollmentStates { get; set; }

    public virtual DbSet<Evidence> Evidences { get; set; }

    public virtual DbSet<EvidenceType> EvidenceTypes { get; set; }

    public virtual DbSet<GroupEnrollment> GroupEnrollments { get; set; }

    public virtual DbSet<MvActivityAnalytic> MvActivityAnalytics { get; set; }

    public virtual DbSet<MvProgramAnalytic> MvProgramAnalytics { get; set; }

    public virtual DbSet<MvScholarshipPerformance> MvScholarshipPerformances { get; set; }

    public virtual DbSet<MvVolunteerHistory> MvVolunteerHistories { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<ProfileState> ProfileStates { get; set; }

    public virtual DbSet<ProgramCollaborator> ProgramCollaborators { get; set; }

    public virtual DbSet<ProgramContent> ProgramContents { get; set; }

    public virtual DbSet<ProgramState> ProgramStates { get; set; }

    public virtual DbSet<RoleRequest> RoleRequests { get; set; }

    public virtual DbSet<RoleRequestState> RoleRequestStates { get; set; }

    public virtual DbSet<TrackingLog> TrackingLogs { get; set; }

    public virtual DbSet<TrackingState> TrackingStates { get; set; }

    public virtual DbSet<TrackingType> TrackingTypes { get; set; }

    public virtual DbSet<UserScholarship> UserScholarships { get; set; }

    public virtual DbSet<VolProgram> VolPrograms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("activities_pkey");

            entity.ToTable("activities");

            entity.HasIndex(e => e.UvaCode).IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.ActivityRecurrencePatternCode).HasColumnName("activity_recurrence_pattern_code");
            entity.Property(e => e.ActivityTypeCode).HasColumnName("activity_type_code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.LocationLatitude).HasColumnName("location_latitude");
            entity.Property(e => e.LocationLongitude).HasColumnName("location_longitude");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.ProgramCode).HasColumnName("program_code");
            entity.Property(e => e.RegistrationRadiusMeters)
                .HasDefaultValue(50)
                .HasColumnName("registration_radius_meters");
            entity.Property(e => e.ResponsibleProfileCode)
                .HasMaxLength(450)
                .HasColumnName("responsible_profile_code");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.StateCode)
                .HasDefaultValue("stage-1")
                .HasColumnName("state_code");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.ActivityRecurrencePattern).WithMany(p => p.Activities)
                .HasForeignKey(d => d.ActivityRecurrencePatternCode)
                .HasPrincipalKey(p => p.UvaCode)
                .HasConstraintName("activities_activity_recurrence_pattern_id_fkey");

            entity.HasOne(d => d.ActivityType).WithMany(p => p.Activities)
                .HasForeignKey(d => d.ActivityTypeCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("activities_activity_type_id_fkey");

            entity.HasOne(d => d.Program).WithMany(p => p.Activities)
                .HasForeignKey(d => d.ProgramCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("activities_program_id_fkey");

            entity.HasOne(d => d.ResponsibleProfile).WithMany(p => p.Activities)
                .HasForeignKey(d => d.ResponsibleProfileCode)
                .HasPrincipalKey(p => p.UvaCode)
                .HasConstraintName("activities_responsible_profile_id_fkey");

            entity.HasOne(d => d.State).WithMany(p => p.Activities)
                .HasForeignKey(d => d.StateCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("activities_state_id_fkey");
        });

        modelBuilder.Entity<ActivityGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("activity_group_pkey");

            entity.ToTable("activity_group");

            entity.HasIndex(e => e.UvaCode).IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.ActivityCode).HasColumnName("activity_code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Details).HasColumnName("details");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.StateCode)
                .HasDefaultValue("stage-1")
                .HasColumnName("state_code");
            entity.Property(e => e.TotalCapacity)
                .HasDefaultValue(0)
                .HasColumnName("total_capacity");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Activity).WithMany(p => p.ActivityGroups)
                .HasForeignKey(d => d.ActivityCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("activity_group_activity_id_fkey");

            entity.HasOne(d => d.State).WithMany(p => p.ActivityGroups)
                .HasForeignKey(d => d.StateCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("activity_group_state_id_fkey");
        });

        modelBuilder.Entity<ActivityRecurrenceDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("activity_recurrence_detail_pkey");

            entity.ToTable("activity_recurrence_detail");

            entity.HasIndex(e => e.UvaCode).IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.ActivityRecurrencePatternCode).HasColumnName("activity_recurrence_pattern_code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DayOfMonth).HasColumnName("day_of_month");
            entity.Property(e => e.DayOfWeek).HasColumnName("day_of_week");
            entity.Property(e => e.EndHour).HasColumnName("end_hour");
            entity.Property(e => e.StartHour).HasColumnName("start_hour");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.WeekOfMonth).HasColumnName("week_of_month");
            entity.Property(e => e.StateCode)
                .HasDefaultValue("stage-1")
                .HasColumnName("state_code");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            entity.HasOne(d => d.ActivityRecurrencePattern).WithMany(p => p.ActivityRecurrenceDetails)
                .HasForeignKey(d => d.ActivityRecurrencePatternCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("activity_recurrence_detail_activity_recurrence_pattern_id_fkey");

            entity.HasOne(d => d.State).WithMany(p => p.ActivityRecurrenceDetails)
                .HasForeignKey(d => d.StateCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("activity_recurrence_detail_state_id_fkey");
        });

        modelBuilder.Entity<ActivityRecurrencePattern>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("activity_recurrence_patterns_pkey");

            entity.ToTable("activity_recurrence_patterns");

            entity.HasIndex(e => e.UvaCode).IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.ProgramCode).HasColumnName("program_code");
            entity.Property(e => e.RecurrenceType)
                .HasMaxLength(20)
                .HasColumnName("recurrence_type");
            entity.Property(e => e.StateCode)
                .HasDefaultValue("stage-1")
                .HasColumnName("state_code");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Program).WithMany(p => p.ActivityRecurrencePatterns)
                .HasForeignKey(d => d.ProgramCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("activity_recurrence_patterns_program_id_fkey");

            entity.HasOne(d => d.State).WithMany(p => p.ActivityRecurrencePatterns)
                .HasForeignKey(d => d.StateCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("activity_recurrence_patterns_state_id_fkey");
        });

        modelBuilder.Entity<ActivityRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("activity_rules_pkey");

            entity.ToTable("activity_rules");

            entity.HasIndex(e => e.UvaCode).IsUnique();
            entity.HasIndex(e => e.ActivityCode, "activity_rules_activity_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.ActivityCode).HasColumnName("activity_code");
            entity.Property(e => e.CostAmount)
                .HasPrecision(10, 2)
                .HasColumnName("cost_amount");
            entity.Property(e => e.CountsVolunteerHours)
                .HasDefaultValue(true)
                .HasColumnName("counts_volunteer_hours");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.EnrollmentDeadline).HasColumnName("enrollment_deadline");
            entity.Property(e => e.PhotoUrl).HasColumnName("photo_url");
            entity.Property(e => e.RequiresApproval)
                .HasDefaultValue(false)
                .HasColumnName("requires_approval");
            entity.Property(e => e.RequiresEnrollment)
                .HasDefaultValue(true)
                .HasColumnName("requires_enrollment");
            entity.Property(e => e.TotalCapacity)
                .HasDefaultValue(0)
                .HasColumnName("total_capacity");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Activity).WithOne(p => p.ActivityRule)
                .HasForeignKey<ActivityRule>(d => d.ActivityCode)
                .HasPrincipalKey<Activity>(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("activity_rules_activity_id_fkey");
        });

        modelBuilder.Entity<ActivityState>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("activity_state_pkey");

            entity.ToTable("activity_state");

            entity.HasIndex(e => e.UvaCode).IsUnique();
            entity.HasIndex(e => e.Name, "activity_state_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ActivityType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("activity_type_pkey");

            entity.ToTable("activity_type");

            entity.HasIndex(e => e.UvaCode).IsUnique();
            entity.HasIndex(e => e.Name, "activity_type_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
        });

        modelBuilder.Entity<CareerType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("career_type_pkey");

            entity.ToTable("career_type");

            entity.HasIndex(e => e.UvaCode).IsUnique();
            entity.HasIndex(e => e.Name, "career_type_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
        });

        modelBuilder.Entity<ScholarshipType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("scholarship_type_pkey");

            entity.ToTable("scholarship_type");

            entity.HasIndex(e => e.UvaCode).IsUnique();
            entity.HasIndex(e => e.Name, "scholarship_type_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
        });

        modelBuilder.Entity<ContractState>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("contract_state_pkey");

            entity.ToTable("contract_state");

            entity.HasIndex(e => e.UvaCode).IsUnique();
            entity.HasIndex(e => e.Name, "contract_state_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("enrollments_pkey");

            entity.ToTable("enrollments");

            entity.HasIndex(e => e.UvaCode).IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.ActivityCode).HasColumnName("activity_code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.EnrolledProfileCode)
                .HasMaxLength(450)
                .HasColumnName("enrolled_profile_code");
            entity.Property(e => e.StateCode)
                .HasDefaultValue("stage-1")
                .HasColumnName("state_code");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Activity).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.ActivityCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("enrollments_activity_id_fkey");

            entity.HasOne(d => d.EnrolledProfile).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.EnrolledProfileCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("enrollments_enrolled_profile_id_fkey");

            entity.HasOne(d => d.State).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.StateCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("enrollments_state_id_fkey");
        });

        modelBuilder.Entity<EnrollmentState>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("enrollment_state_pkey");

            entity.ToTable("enrollment_state");

            entity.HasIndex(e => e.UvaCode).IsUnique();
            entity.HasIndex(e => e.Name, "enrollment_state_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Evidence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("evidences_pkey");

            entity.ToTable("evidences");

            entity.HasIndex(e => e.UvaCode).IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.EvidenceTypeCode).HasColumnName("evidence_type_code");
            entity.Property(e => e.LocationLatitude).HasColumnName("location_latitude");
            entity.Property(e => e.LocationLongitude).HasColumnName("location_longitude");
            entity.Property(e => e.Observations).HasColumnName("observations");
            entity.Property(e => e.PhotoUrl).HasColumnName("photo_url");
            entity.Property(e => e.TrackingLogCode).HasColumnName("tracking_log_code");
            entity.Property(e => e.TypeCode).HasColumnName("type_code");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.EvidenceType).WithMany(p => p.Evidences)
                .HasForeignKey(d => d.EvidenceTypeCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("evidences_evidence_type_id_fkey");

            entity.HasOne(d => d.TrackingLog).WithMany(p => p.Evidences)
                .HasForeignKey(d => d.TrackingLogCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("evidences_tracking_log_id_fkey");

            entity.HasOne(d => d.Type).WithMany(p => p.Evidences)
                .HasForeignKey(d => d.TypeCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("evidences_type_id_fkey");
        });

        modelBuilder.Entity<EvidenceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("evidence_type_pkey");

            entity.ToTable("evidence_type");

            entity.HasIndex(e => e.UvaCode).IsUnique();
            entity.HasIndex(e => e.Name, "evidence_type_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
        });

        modelBuilder.Entity<GroupEnrollment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("group_enrollment_pkey");

            entity.ToTable("group_enrollment");

            entity.HasIndex(e => e.UvaCode).IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.ActivityGroupCode).HasColumnName("activity_group_code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.EnrollmentCode).HasColumnName("enrollment_code");
            entity.Property(e => e.StateCode)
                .HasDefaultValue("stage-1")
                .HasColumnName("state_code");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.ActivityGroup).WithMany(p => p.GroupEnrollments)
                .HasForeignKey(d => d.ActivityGroupCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("group_enrollment_activity_group_id_fkey");

            entity.HasOne(d => d.Enrollment).WithMany(p => p.GroupEnrollments)
                .HasForeignKey(d => d.EnrollmentCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("group_enrollment_enrollment_id_fkey");

            entity.HasOne(d => d.State).WithMany(p => p.GroupEnrollments)
                .HasForeignKey(d => d.StateCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("group_enrollment_state_id_fkey");
        });

        modelBuilder.Entity<MvActivityAnalytic>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("mv_activity_analytics");

            entity.Property(e => e.ActivityCode).HasColumnName("activity_code");
            entity.Property(e => e.ActivityName).HasColumnName("activity_name");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.ProgramCode).HasColumnName("program_code");
            entity.Property(e => e.ProgramName).HasColumnName("program_name");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.TotalActivityHours).HasColumnName("total_activity_hours");
            entity.Property(e => e.TotalAttended).HasColumnName("total_attended");
            entity.Property(e => e.TotalCapacity).HasColumnName("total_capacity");
            entity.Property(e => e.TotalEnrolled).HasColumnName("total_enrolled");
        });

        modelBuilder.Entity<MvProgramAnalytic>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("mv_program_analytics");

            entity.Property(e => e.ProgramCode).HasColumnName("program_code");
            entity.Property(e => e.ProgramName).HasColumnName("program_name");
            entity.Property(e => e.TotalActivities).HasColumnName("total_activities");
            entity.Property(e => e.TotalGeneratedHours).HasColumnName("total_generated_hours");
            entity.Property(e => e.TotalUniqueVolunteers).HasColumnName("total_unique_volunteers");
        });

        modelBuilder.Entity<MvScholarshipPerformance>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("mv_scholarship_performance");

            entity.Property(e => e.CompletedHours).HasColumnName("completed_hours");
            entity.Property(e => e.CompletionPercentage).HasColumnName("completion_percentage");
            entity.Property(e => e.ContractState)
                .HasMaxLength(50)
                .HasColumnName("contract_state");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.ProfileCode).HasColumnName("profile_code");
            entity.Property(e => e.RemainingHours).HasColumnName("remaining_hours");
            entity.Property(e => e.RequiredHours)
                .HasPrecision(5, 2)
                .HasColumnName("required_hours");
            entity.Property(e => e.ScholarshipCode).HasColumnName("scholarship_code");
            entity.Property(e => e.ScholarshipType)
                .HasMaxLength(50)
                .HasColumnName("scholarship_type");
        });

        modelBuilder.Entity<MvVolunteerHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("mv_volunteer_history");

            entity.Property(e => e.CareerName)
                .HasMaxLength(50)
                .HasColumnName("career_name");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.LastActivityDate).HasColumnName("last_activity_date");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.PersonalGoalHours)
                .HasPrecision(5, 2)
                .HasColumnName("personal_goal_hours");
            entity.Property(e => e.ProfileCode).HasColumnName("profile_code");
            entity.Property(e => e.TotalActivitiesParticipated).HasColumnName("total_activities_participated");
            entity.Property(e => e.TotalLoggedHours).HasColumnName("total_logged_hours");
            entity.Property(e => e.ValidatedHours).HasColumnName("validated_hours");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("profiles_pkey");

            entity.ToTable("profiles");

            entity.HasIndex(e => e.UvaCode).IsUnique();
            entity.HasIndex(e => e.Email, "profiles_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.IdentityUserId)
                .HasMaxLength(450)
                .HasColumnName("identity_user_id");
            entity.Property(e => e.AddressLocation).HasColumnName("address_location");
            entity.Property(e => e.CareerCode).HasColumnName("career_code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.PersonalGoalHours)
                .HasPrecision(5, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("personal_goal_hours");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.PhotoUrl).HasColumnName("photo_url");
            entity.Property(e => e.StateCode)
                .HasDefaultValue("stage-1")
                .HasColumnName("state_code");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.IdentityUser).WithOne()
                .HasForeignKey<Profile>(d => d.IdentityUserId)
                .HasConstraintName("profiles_identity_user_id_fkey");

            entity.HasOne(d => d.Career).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.CareerCode)
                .HasPrincipalKey(p => p.UvaCode)
                .HasConstraintName("profiles_career_id_fkey");

            entity.HasOne(d => d.State).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.StateCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("profiles_state_id_fkey");
        });

        modelBuilder.Entity<ProfileState>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("profile_state_pkey");

            entity.ToTable("profile_state");

            entity.HasIndex(e => e.UvaCode).IsUnique();
            entity.HasIndex(e => e.Name, "profile_state_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ProgramCollaborator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("program_collaborators_pkey");

            entity.ToTable("program_collaborators");

            entity.HasIndex(e => e.UvaCode).IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.AssignedByProfileCode)
                .HasMaxLength(450)
                .HasColumnName("assigned_by_profile_code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.ProfileCode)
                .HasMaxLength(450)
                .HasColumnName("profile_code");
            entity.Property(e => e.ProgramCode).HasColumnName("program_code");
            entity.Property(e => e.StateCode)
                .HasDefaultValue("stage-1")
                .HasColumnName("state_code");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.AssignedByProfile).WithMany(p => p.ProgramCollaboratorAssignedByProfiles)
                .HasForeignKey(d => d.AssignedByProfileCode)
                .HasPrincipalKey(p => p.UvaCode)
                .HasConstraintName("program_collaborators_assigned_by_profile_id_fkey");

            entity.HasOne(d => d.Profile).WithMany(p => p.ProgramCollaboratorProfiles)
                .HasForeignKey(d => d.ProfileCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("program_collaborators_profile_id_fkey");

            entity.HasOne(d => d.Program).WithMany(p => p.ProgramCollaborators)
                .HasForeignKey(d => d.ProgramCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("program_collaborators_program_id_fkey");

            entity.HasOne(d => d.State).WithMany(p => p.ProgramCollaborators)
                .HasForeignKey(d => d.StateCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("program_collaborators_state_id_fkey");
        });

        modelBuilder.Entity<ProgramContent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("program_content_pkey");

            entity.ToTable("program_content");

            entity.HasIndex(e => e.UvaCode).IsUnique();
            entity.HasIndex(e => e.ProgramCode, "program_content_program_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.ActivitiesDescription).HasColumnName("activities_description");
            entity.Property(e => e.ContactInfo).HasColumnName("contact_info");
            entity.Property(e => e.CoverPhotoUrl).HasColumnName("cover_photo_url");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.LeadershipInfo).HasColumnName("leadership_info");
            entity.Property(e => e.MissionStatement).HasColumnName("mission_statement");
            entity.Property(e => e.ProfilePhotoUrl).HasColumnName("profile_photo_url");
            entity.Property(e => e.ProgramCode).HasColumnName("program_code");
            entity.Property(e => e.ScheduleInfo).HasColumnName("schedule_info");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Program).WithOne(p => p.ProgramContent)
                .HasForeignKey<ProgramContent>(d => d.ProgramCode)
                .HasPrincipalKey<VolProgram>(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("program_content_program_id_fkey");
        });

        modelBuilder.Entity<ProgramState>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("program_state_pkey");

            entity.ToTable("program_state");

            entity.HasIndex(e => e.UvaCode).IsUnique();
            entity.HasIndex(e => e.Name, "program_state_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<RoleRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_requests_pkey");

            entity.ToTable("role_requests");

            entity.HasIndex(e => e.UvaCode).IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DurationInMonths).HasColumnName("duration_in_months");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.RequestedRoleId)
                .HasMaxLength(450)
                .HasColumnName("requested_role_id");
            entity.Property(e => e.RequesterProfileCode)
                .HasMaxLength(450)
                .HasColumnName("requester_profile_code");
            entity.Property(e => e.ResolvedAt).HasColumnName("resolved_at");
            entity.Property(e => e.ResolvedByProfileCode)
                .HasMaxLength(450)
                .HasColumnName("resolved_by_profile_code");
            entity.Property(e => e.StateCode)
                .HasDefaultValue("stage-1")
                .HasColumnName("state_code");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.RequesterProfile).WithMany(p => p.RoleRequestRequesterProfiles)
                .HasForeignKey(d => d.RequesterProfileCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("role_requests_requester_profile_id_fkey");

            entity.HasOne(d => d.ResolvedByProfile).WithMany(p => p.RoleRequestResolvedByProfiles)
                .HasForeignKey(d => d.ResolvedByProfileCode)
                .HasPrincipalKey(p => p.UvaCode)
                .HasConstraintName("role_requests_resolved_by_profile_id_fkey");

            entity.HasOne(d => d.RequestedRole).WithMany()
                .HasForeignKey(d => d.RequestedRoleId)
                .HasConstraintName("role_requests_requested_role_id_fkey");

            entity.HasOne(d => d.State).WithMany(p => p.RoleRequests)
                .HasForeignKey(d => d.StateCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("role_requests_state_id_fkey");
        });

        modelBuilder.Entity<RoleRequestState>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_request_state_pkey");

            entity.ToTable("role_request_state");

            entity.HasIndex(e => e.UvaCode).IsUnique();
            entity.HasIndex(e => e.Name, "role_request_state_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TrackingLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tracking_logs_pkey");

            entity.ToTable("tracking_logs");

            entity.HasIndex(e => e.UvaCode).IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.CalculatedHours)
                .HasPrecision(5, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("calculated_hours");
            entity.Property(e => e.CheckInRegisteredByCode)
                .HasMaxLength(450)
                .HasColumnName("check_in_registered_by_code");
            entity.Property(e => e.CheckOutRegisteredByCode)
                .HasMaxLength(450)
                .HasColumnName("check_out_registered_by_code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.EnrollmentCode).HasColumnName("enrollment_code");
            entity.Property(e => e.GroupEnrollmentCode).HasColumnName("group_enrollment_code");
            entity.Property(e => e.EntryTime).HasColumnName("entry_time");
            entity.Property(e => e.ExitTime).HasColumnName("exit_time");
            entity.Property(e => e.StateCode)
                .HasDefaultValue("stage-1")
                .HasColumnName("state_code");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.CheckInRegisteredBy).WithMany(p => p.TrackingLogCheckInRegisteredBies)
                .HasForeignKey(d => d.CheckInRegisteredByCode)
                .HasPrincipalKey(p => p.UvaCode)
                .HasConstraintName("tracking_logs_check_in_registered_by_id_fkey");

            entity.HasOne(d => d.CheckOutRegisteredBy).WithMany(p => p.TrackingLogCheckOutRegisteredBies)
                .HasForeignKey(d => d.CheckOutRegisteredByCode)
                .HasPrincipalKey(p => p.UvaCode)
                .HasConstraintName("tracking_logs_check_out_registered_by_id_fkey");

            entity.HasOne(d => d.GroupEnrollment).WithMany(p => p.TrackingLogs)
                .HasForeignKey(d => d.GroupEnrollmentCode)
                .HasPrincipalKey(p => p.UvaCode)
                .HasConstraintName("tracking_logs_group_enrollment_id_fkey");

            entity.HasOne(d => d.Enrollment).WithMany(p => p.TrackingLogs)
                .HasForeignKey(d => d.EnrollmentCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tracking_logs_enrollment_id_fkey");

            entity.HasOne(d => d.State).WithMany(p => p.TrackingLogs)
                .HasForeignKey(d => d.StateCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tracking_logs_state_id_fkey");
        });

        modelBuilder.Entity<TrackingState>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tracking_state_pkey");

            entity.ToTable("tracking_state");

            entity.HasIndex(e => e.UvaCode).IsUnique();
            entity.HasIndex(e => e.Name, "tracking_state_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TrackingType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tracking_type_pkey");

            entity.ToTable("tracking_type");

            entity.HasIndex(e => e.UvaCode).IsUnique();
            entity.HasIndex(e => e.Name, "tracking_type_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
        });

        modelBuilder.Entity<UserScholarship>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_scholarships_pkey");

            entity.ToTable("user_scholarships");

            entity.HasIndex(e => e.UvaCode).IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.AssignedProfileCode)
                .HasMaxLength(450)
                .HasColumnName("assigned_profile_code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.EvaluatorProfileCode)
                .HasMaxLength(450)
                .HasColumnName("evaluator_profile_code");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.RequiredHours)
                .HasPrecision(5, 2)
                .HasDefaultValueSql("100.00")
                .HasColumnName("required_hours");
            entity.Property(e => e.ScholarshipTypeCode).HasColumnName("scholarship_type_code");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("now()")
                .HasColumnName("start_date");
            entity.Property(e => e.StateCode)
                .HasDefaultValue("stage-1")
                .HasColumnName("state_code");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.AssignedProfile).WithMany(p => p.UserScholarshipAssignedProfiles)
                .HasForeignKey(d => d.AssignedProfileCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_scholarships_assigned_profile_id_fkey");

            entity.HasOne(d => d.EvaluatorProfile).WithMany(p => p.UserScholarshipEvaluatorProfiles)
                .HasForeignKey(d => d.EvaluatorProfileCode)
                .HasPrincipalKey(p => p.UvaCode)
                .HasConstraintName("user_scholarships_evaluator_profile_id_fkey");

            entity.HasOne(d => d.ScholarshipType).WithMany(p => p.UserScholarships)
                .HasForeignKey(d => d.ScholarshipTypeCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_scholarships_scholarship_type_id_fkey");

            entity.HasOne(d => d.State).WithMany(p => p.UserScholarships)
                .HasForeignKey(d => d.StateCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_scholarships_state_id_fkey");
        });

        modelBuilder.Entity<VolProgram>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("vol_programs_pkey");

            entity.ToTable("vol_programs");

            entity.HasIndex(e => e.UvaCode).IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.Acronym).HasColumnName("acronym");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.ManagerProfileCode)
                .HasMaxLength(450)
                .HasColumnName("manager_profile_code");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.StateCode)
                .HasDefaultValue("stage-1")
                .HasColumnName("state_code");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.ManagerProfile).WithMany(p => p.VolPrograms)
                .HasForeignKey(d => d.ManagerProfileCode)
                .HasPrincipalKey(p => p.UvaCode)
                .HasConstraintName("vol_programs_manager_profile_id_fkey");

            entity.HasOne(d => d.State).WithMany(p => p.VolPrograms)
                .HasForeignKey(d => d.StateCode)
                .HasPrincipalKey(p => p.UvaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vol_programs_state_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
