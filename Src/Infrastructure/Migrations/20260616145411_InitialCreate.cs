using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace U_VoluntApp_Core.Src.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uva_code = table.Column<string>(type: "text", nullable: false),
                    identity_user_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    profile_code = table.Column<string>(type: "text", nullable: false),
                    token_hash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    revoked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    replaced_by_token_hash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    created_by_ip = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    revoked_by_ip = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    user_agent = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    reason_revoked = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("refresh_tokens_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "profiles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uva_code = table.Column<string>(type: "text", nullable: false),
                    identity_user_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    photo_url = table.Column<string>(type: "text", nullable: true),
                    career_code = table.Column<string>(type: "text", nullable: false),
                    address_location = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true),
                    personal_goal_hours = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false, defaultValueSql: "0.00"),
                    state_code = table.Column<string>(type: "text", nullable: false, defaultValue: "stage-1"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("profiles_pkey", x => x.id);
                    table.UniqueConstraint("AK_profiles_uva_code", x => x.uva_code);
                    table.ForeignKey(
                        name: "profiles_identity_user_id_fkey",
                        column: x => x.identity_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_requests",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uva_code = table.Column<string>(type: "text", nullable: false),
                    requester_profile_code = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    requested_role_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    reason = table.Column<string>(type: "text", nullable: false),
                    duration_in_months = table.Column<int>(type: "integer", nullable: true),
                    state_code = table.Column<string>(type: "text", nullable: false, defaultValue: "stage-1"),
                    resolved_by_profile_code = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    resolved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("role_requests_pkey", x => x.id);
                    table.ForeignKey(
                        name: "role_requests_requested_role_id_fkey",
                        column: x => x.requested_role_id,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "role_requests_requester_profile_id_fkey",
                        column: x => x.requester_profile_code,
                        principalTable: "profiles",
                        principalColumn: "uva_code");
                    table.ForeignKey(
                        name: "role_requests_resolved_by_profile_id_fkey",
                        column: x => x.resolved_by_profile_code,
                        principalTable: "profiles",
                        principalColumn: "uva_code");
                });

            migrationBuilder.CreateTable(
                name: "user_scholarships",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uva_code = table.Column<string>(type: "text", nullable: false),
                    assigned_profile_code = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    evaluator_profile_code = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    scholarship_type_code = table.Column<string>(type: "text", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: false),
                    required_hours = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false, defaultValueSql: "100.00"),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    state_code = table.Column<string>(type: "text", nullable: false, defaultValue: "stage-1"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_scholarships_pkey", x => x.id);
                    table.ForeignKey(
                        name: "user_scholarships_assigned_profile_id_fkey",
                        column: x => x.assigned_profile_code,
                        principalTable: "profiles",
                        principalColumn: "uva_code");
                    table.ForeignKey(
                        name: "user_scholarships_evaluator_profile_id_fkey",
                        column: x => x.evaluator_profile_code,
                        principalTable: "profiles",
                        principalColumn: "uva_code");
                });

            migrationBuilder.CreateTable(
                name: "vol_programs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uva_code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    acronym = table.Column<string>(type: "text", nullable: true),
                    manager_profile_code = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    state_code = table.Column<string>(type: "text", nullable: false, defaultValue: "stage-1"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("vol_programs_pkey", x => x.id);
                    table.UniqueConstraint("AK_vol_programs_uva_code", x => x.uva_code);
                    table.ForeignKey(
                        name: "vol_programs_manager_profile_id_fkey",
                        column: x => x.manager_profile_code,
                        principalTable: "profiles",
                        principalColumn: "uva_code");
                });

            migrationBuilder.CreateTable(
                name: "activity_recurrence_patterns",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uva_code = table.Column<string>(type: "text", nullable: false),
                    program_code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    recurrence_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    state_code = table.Column<string>(type: "text", nullable: false, defaultValue: "stage-1"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("activity_recurrence_patterns_pkey", x => x.id);
                    table.UniqueConstraint("AK_activity_recurrence_patterns_uva_code", x => x.uva_code);
                    table.ForeignKey(
                        name: "activity_recurrence_patterns_program_id_fkey",
                        column: x => x.program_code,
                        principalTable: "vol_programs",
                        principalColumn: "uva_code");
                });

            migrationBuilder.CreateTable(
                name: "program_collaborators",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uva_code = table.Column<string>(type: "text", nullable: false),
                    program_code = table.Column<string>(type: "text", nullable: false),
                    profile_code = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    assigned_by_profile_code = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    state_code = table.Column<string>(type: "text", nullable: false, defaultValue: "stage-1"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("program_collaborators_pkey", x => x.id);
                    table.ForeignKey(
                        name: "program_collaborators_assigned_by_profile_id_fkey",
                        column: x => x.assigned_by_profile_code,
                        principalTable: "profiles",
                        principalColumn: "uva_code");
                    table.ForeignKey(
                        name: "program_collaborators_profile_id_fkey",
                        column: x => x.profile_code,
                        principalTable: "profiles",
                        principalColumn: "uva_code");
                    table.ForeignKey(
                        name: "program_collaborators_program_id_fkey",
                        column: x => x.program_code,
                        principalTable: "vol_programs",
                        principalColumn: "uva_code");
                });

            migrationBuilder.CreateTable(
                name: "program_content",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uva_code = table.Column<string>(type: "text", nullable: false),
                    program_code = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    activities_description = table.Column<string>(type: "text", nullable: true),
                    schedule_info = table.Column<string>(type: "text", nullable: true),
                    leadership_info = table.Column<string>(type: "text", nullable: true),
                    contact_info = table.Column<string>(type: "text", nullable: true),
                    mission_statement = table.Column<string>(type: "text", nullable: true),
                    profile_photo_url = table.Column<string>(type: "text", nullable: true),
                    cover_photo_url = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("program_content_pkey", x => x.id);
                    table.ForeignKey(
                        name: "program_content_program_id_fkey",
                        column: x => x.program_code,
                        principalTable: "vol_programs",
                        principalColumn: "uva_code");
                });

            migrationBuilder.CreateTable(
                name: "activities",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uva_code = table.Column<string>(type: "text", nullable: false),
                    program_code = table.Column<string>(type: "text", nullable: false),
                    responsible_profile_code = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    activity_type_code = table.Column<string>(type: "text", nullable: false),
                    activity_recurrence_pattern_code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    location_latitude = table.Column<double>(type: "double precision", nullable: false),
                    location_longitude = table.Column<double>(type: "double precision", nullable: false),
                    registration_radius_meters = table.Column<int>(type: "integer", nullable: false, defaultValue: 50),
                    state_code = table.Column<string>(type: "text", nullable: false, defaultValue: "stage-1"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("activities_pkey", x => x.id);
                    table.UniqueConstraint("AK_activities_uva_code", x => x.uva_code);
                    table.ForeignKey(
                        name: "activities_activity_recurrence_pattern_id_fkey",
                        column: x => x.activity_recurrence_pattern_code,
                        principalTable: "activity_recurrence_patterns",
                        principalColumn: "uva_code");
                    table.ForeignKey(
                        name: "activities_program_id_fkey",
                        column: x => x.program_code,
                        principalTable: "vol_programs",
                        principalColumn: "uva_code");
                    table.ForeignKey(
                        name: "activities_responsible_profile_id_fkey",
                        column: x => x.responsible_profile_code,
                        principalTable: "profiles",
                        principalColumn: "uva_code");
                });

            migrationBuilder.CreateTable(
                name: "activity_recurrence_detail",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uva_code = table.Column<string>(type: "text", nullable: false),
                    activity_recurrence_pattern_code = table.Column<string>(type: "text", nullable: false),
                    day_of_week = table.Column<short>(type: "smallint", nullable: true),
                    day_of_month = table.Column<short>(type: "smallint", nullable: true),
                    week_of_month = table.Column<short>(type: "smallint", nullable: true),
                    start_hour = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    end_hour = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    state_code = table.Column<string>(type: "text", nullable: false, defaultValue: "stage-1"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("activity_recurrence_detail_pkey", x => x.id);
                    table.ForeignKey(
                        name: "activity_recurrence_detail_activity_recurrence_pattern_id_fkey",
                        column: x => x.activity_recurrence_pattern_code,
                        principalTable: "activity_recurrence_patterns",
                        principalColumn: "uva_code");
                });

            migrationBuilder.CreateTable(
                name: "activity_group",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uva_code = table.Column<string>(type: "text", nullable: false),
                    activity_code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    details = table.Column<string>(type: "text", nullable: true),
                    total_capacity = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    state_code = table.Column<string>(type: "text", nullable: false, defaultValue: "stage-1"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("activity_group_pkey", x => x.id);
                    table.UniqueConstraint("AK_activity_group_uva_code", x => x.uva_code);
                    table.ForeignKey(
                        name: "activity_group_activity_id_fkey",
                        column: x => x.activity_code,
                        principalTable: "activities",
                        principalColumn: "uva_code");
                });

            migrationBuilder.CreateTable(
                name: "activity_rules",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uva_code = table.Column<string>(type: "text", nullable: false),
                    activity_code = table.Column<string>(type: "text", nullable: false),
                    requires_enrollment = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    enrollment_deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    requires_approval = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    total_capacity = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    cost_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    counts_volunteer_hours = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    photo_url = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("activity_rules_pkey", x => x.id);
                    table.ForeignKey(
                        name: "activity_rules_activity_id_fkey",
                        column: x => x.activity_code,
                        principalTable: "activities",
                        principalColumn: "uva_code");
                });

            migrationBuilder.CreateTable(
                name: "enrollments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uva_code = table.Column<string>(type: "text", nullable: false),
                    activity_code = table.Column<string>(type: "text", nullable: false),
                    enrolled_profile_code = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    state_code = table.Column<string>(type: "text", nullable: false, defaultValue: "stage-1"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("enrollments_pkey", x => x.id);
                    table.UniqueConstraint("AK_enrollments_uva_code", x => x.uva_code);
                    table.ForeignKey(
                        name: "enrollments_activity_id_fkey",
                        column: x => x.activity_code,
                        principalTable: "activities",
                        principalColumn: "uva_code");
                    table.ForeignKey(
                        name: "enrollments_enrolled_profile_id_fkey",
                        column: x => x.enrolled_profile_code,
                        principalTable: "profiles",
                        principalColumn: "uva_code");
                });

            migrationBuilder.CreateTable(
                name: "group_enrollment",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uva_code = table.Column<string>(type: "text", nullable: false),
                    activity_group_code = table.Column<string>(type: "text", nullable: false),
                    enrollment_code = table.Column<string>(type: "text", nullable: false),
                    state_code = table.Column<string>(type: "text", nullable: false, defaultValue: "stage-1"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("group_enrollment_pkey", x => x.id);
                    table.UniqueConstraint("AK_group_enrollment_uva_code", x => x.uva_code);
                    table.ForeignKey(
                        name: "group_enrollment_activity_group_id_fkey",
                        column: x => x.activity_group_code,
                        principalTable: "activity_group",
                        principalColumn: "uva_code");
                    table.ForeignKey(
                        name: "group_enrollment_enrollment_id_fkey",
                        column: x => x.enrollment_code,
                        principalTable: "enrollments",
                        principalColumn: "uva_code");
                });

            migrationBuilder.CreateTable(
                name: "tracking_logs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uva_code = table.Column<string>(type: "text", nullable: false),
                    enrollment_code = table.Column<string>(type: "text", nullable: false),
                    group_enrollment_code = table.Column<string>(type: "text", nullable: true),
                    entry_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    exit_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    calculated_hours = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false, defaultValueSql: "0.00"),
                    state_code = table.Column<string>(type: "text", nullable: false, defaultValue: "stage-1"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    check_in_registered_by_code = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    check_out_registered_by_code = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tracking_logs_pkey", x => x.id);
                    table.UniqueConstraint("AK_tracking_logs_uva_code", x => x.uva_code);
                    table.ForeignKey(
                        name: "tracking_logs_check_in_registered_by_id_fkey",
                        column: x => x.check_in_registered_by_code,
                        principalTable: "profiles",
                        principalColumn: "uva_code");
                    table.ForeignKey(
                        name: "tracking_logs_check_out_registered_by_id_fkey",
                        column: x => x.check_out_registered_by_code,
                        principalTable: "profiles",
                        principalColumn: "uva_code");
                    table.ForeignKey(
                        name: "tracking_logs_enrollment_id_fkey",
                        column: x => x.enrollment_code,
                        principalTable: "enrollments",
                        principalColumn: "uva_code");
                    table.ForeignKey(
                        name: "tracking_logs_group_enrollment_id_fkey",
                        column: x => x.group_enrollment_code,
                        principalTable: "group_enrollment",
                        principalColumn: "uva_code");
                });

            migrationBuilder.CreateTable(
                name: "evidences",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uva_code = table.Column<string>(type: "text", nullable: false),
                    tracking_log_code = table.Column<string>(type: "text", nullable: false),
                    photo_url = table.Column<string>(type: "text", nullable: false),
                    evidence_type_code = table.Column<string>(type: "text", nullable: false),
                    type_code = table.Column<string>(type: "text", nullable: false),
                    observations = table.Column<string>(type: "text", nullable: true),
                    location_latitude = table.Column<double>(type: "double precision", nullable: false),
                    location_longitude = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("evidences_pkey", x => x.id);
                    table.ForeignKey(
                        name: "evidences_tracking_log_id_fkey",
                        column: x => x.tracking_log_code,
                        principalTable: "tracking_logs",
                        principalColumn: "uva_code");
                });

            migrationBuilder.CreateIndex(
                name: "IX_activities_activity_recurrence_pattern_code",
                table: "activities",
                column: "activity_recurrence_pattern_code");

            migrationBuilder.CreateIndex(
                name: "IX_activities_program_code",
                table: "activities",
                column: "program_code");

            migrationBuilder.CreateIndex(
                name: "IX_activities_responsible_profile_code",
                table: "activities",
                column: "responsible_profile_code");

            migrationBuilder.CreateIndex(
                name: "IX_activities_uva_code",
                table: "activities",
                column: "uva_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_activity_group_activity_code",
                table: "activity_group",
                column: "activity_code");

            migrationBuilder.CreateIndex(
                name: "IX_activity_group_uva_code",
                table: "activity_group",
                column: "uva_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_activity_recurrence_detail_activity_recurrence_pattern_code",
                table: "activity_recurrence_detail",
                column: "activity_recurrence_pattern_code");

            migrationBuilder.CreateIndex(
                name: "IX_activity_recurrence_detail_uva_code",
                table: "activity_recurrence_detail",
                column: "uva_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_activity_recurrence_patterns_program_code",
                table: "activity_recurrence_patterns",
                column: "program_code");

            migrationBuilder.CreateIndex(
                name: "IX_activity_recurrence_patterns_uva_code",
                table: "activity_recurrence_patterns",
                column: "uva_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "activity_rules_activity_id_key",
                table: "activity_rules",
                column: "activity_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_activity_rules_uva_code",
                table: "activity_rules",
                column: "uva_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_enrollments_activity_code",
                table: "enrollments",
                column: "activity_code");

            migrationBuilder.CreateIndex(
                name: "IX_enrollments_enrolled_profile_code",
                table: "enrollments",
                column: "enrolled_profile_code");

            migrationBuilder.CreateIndex(
                name: "IX_enrollments_uva_code",
                table: "enrollments",
                column: "uva_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_evidences_tracking_log_code",
                table: "evidences",
                column: "tracking_log_code");

            migrationBuilder.CreateIndex(
                name: "IX_evidences_uva_code",
                table: "evidences",
                column: "uva_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_group_enrollment_activity_group_code",
                table: "group_enrollment",
                column: "activity_group_code");

            migrationBuilder.CreateIndex(
                name: "IX_group_enrollment_enrollment_code",
                table: "group_enrollment",
                column: "enrollment_code");

            migrationBuilder.CreateIndex(
                name: "IX_group_enrollment_uva_code",
                table: "group_enrollment",
                column: "uva_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_profiles_identity_user_id",
                table: "profiles",
                column: "identity_user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_profiles_uva_code",
                table: "profiles",
                column: "uva_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "profiles_email_key",
                table: "profiles",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_program_collaborators_assigned_by_profile_code",
                table: "program_collaborators",
                column: "assigned_by_profile_code");

            migrationBuilder.CreateIndex(
                name: "IX_program_collaborators_profile_code",
                table: "program_collaborators",
                column: "profile_code");

            migrationBuilder.CreateIndex(
                name: "IX_program_collaborators_program_code",
                table: "program_collaborators",
                column: "program_code");

            migrationBuilder.CreateIndex(
                name: "IX_program_collaborators_uva_code",
                table: "program_collaborators",
                column: "uva_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_program_content_uva_code",
                table: "program_content",
                column: "uva_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "program_content_program_id_key",
                table: "program_content",
                column: "program_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_identity_user_id",
                table: "refresh_tokens",
                column: "identity_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_profile_code",
                table: "refresh_tokens",
                column: "profile_code");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_token_hash",
                table: "refresh_tokens",
                column: "token_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_uva_code",
                table: "refresh_tokens",
                column: "uva_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_requests_requested_role_id",
                table: "role_requests",
                column: "requested_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_requests_requester_profile_code",
                table: "role_requests",
                column: "requester_profile_code");

            migrationBuilder.CreateIndex(
                name: "IX_role_requests_resolved_by_profile_code",
                table: "role_requests",
                column: "resolved_by_profile_code");

            migrationBuilder.CreateIndex(
                name: "IX_role_requests_uva_code",
                table: "role_requests",
                column: "uva_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tracking_logs_check_in_registered_by_code",
                table: "tracking_logs",
                column: "check_in_registered_by_code");

            migrationBuilder.CreateIndex(
                name: "IX_tracking_logs_check_out_registered_by_code",
                table: "tracking_logs",
                column: "check_out_registered_by_code");

            migrationBuilder.CreateIndex(
                name: "IX_tracking_logs_enrollment_code",
                table: "tracking_logs",
                column: "enrollment_code");

            migrationBuilder.CreateIndex(
                name: "IX_tracking_logs_group_enrollment_code",
                table: "tracking_logs",
                column: "group_enrollment_code");

            migrationBuilder.CreateIndex(
                name: "IX_tracking_logs_uva_code",
                table: "tracking_logs",
                column: "uva_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_scholarships_assigned_profile_code",
                table: "user_scholarships",
                column: "assigned_profile_code");

            migrationBuilder.CreateIndex(
                name: "IX_user_scholarships_evaluator_profile_code",
                table: "user_scholarships",
                column: "evaluator_profile_code");

            migrationBuilder.CreateIndex(
                name: "IX_user_scholarships_uva_code",
                table: "user_scholarships",
                column: "uva_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vol_programs_manager_profile_code",
                table: "vol_programs",
                column: "manager_profile_code");

            migrationBuilder.CreateIndex(
                name: "IX_vol_programs_uva_code",
                table: "vol_programs",
                column: "uva_code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activity_recurrence_detail");

            migrationBuilder.DropTable(
                name: "activity_rules");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "evidences");

            migrationBuilder.DropTable(
                name: "program_collaborators");

            migrationBuilder.DropTable(
                name: "program_content");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "role_requests");

            migrationBuilder.DropTable(
                name: "user_scholarships");

            migrationBuilder.DropTable(
                name: "tracking_logs");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "group_enrollment");

            migrationBuilder.DropTable(
                name: "activity_group");

            migrationBuilder.DropTable(
                name: "enrollments");

            migrationBuilder.DropTable(
                name: "activities");

            migrationBuilder.DropTable(
                name: "activity_recurrence_patterns");

            migrationBuilder.DropTable(
                name: "vol_programs");

            migrationBuilder.DropTable(
                name: "profiles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
