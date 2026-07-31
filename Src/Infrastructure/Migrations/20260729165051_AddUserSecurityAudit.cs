using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace U_VoluntApp_Core.Src.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSecurityAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "activities_activity_recurrence_pattern_id_fkey",
                table: "activities");

            migrationBuilder.DropTable(
                name: "activity_recurrence_detail");

            migrationBuilder.DropTable(
                name: "program_collaborators");

            migrationBuilder.DropTable(
                name: "program_content");

            migrationBuilder.DropTable(
                name: "activity_recurrence_patterns");

            migrationBuilder.RenameColumn(
                name: "activity_recurrence_pattern_code",
                table: "activities",
                newName: "vol_program_pattern_code");

            migrationBuilder.RenameIndex(
                name: "IX_activities_activity_recurrence_pattern_code",
                table: "activities",
                newName: "IX_activities_vol_program_pattern_code");

            migrationBuilder.CreateTable(
                name: "user_security_audits",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uva_code = table.Column<string>(type: "text", nullable: false),
                    profile_code = table.Column<string>(type: "text", nullable: false),
                    last_ip_address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    device_fingerprint = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    last_code_sent_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_trusted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_security_audits_pkey", x => x.id);
                    table.ForeignKey(
                        name: "user_security_audits_profile_id_fkey",
                        column: x => x.profile_code,
                        principalTable: "profiles",
                        principalColumn: "uva_code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vol_program_collaborators",
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
                    table.PrimaryKey("vol_program_collaborators_pkey", x => x.id);
                    table.ForeignKey(
                        name: "vol_program_collaborators_assigned_by_profile_id_fkey",
                        column: x => x.assigned_by_profile_code,
                        principalTable: "profiles",
                        principalColumn: "uva_code");
                    table.ForeignKey(
                        name: "vol_program_collaborators_profile_id_fkey",
                        column: x => x.profile_code,
                        principalTable: "profiles",
                        principalColumn: "uva_code");
                    table.ForeignKey(
                        name: "vol_program_collaborators_program_id_fkey",
                        column: x => x.program_code,
                        principalTable: "vol_programs",
                        principalColumn: "uva_code");
                });

            migrationBuilder.CreateTable(
                name: "vol_program_contents",
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
                    table.PrimaryKey("vol_program_contents_pkey", x => x.id);
                    table.ForeignKey(
                        name: "vol_program_contents_program_id_fkey",
                        column: x => x.program_code,
                        principalTable: "vol_programs",
                        principalColumn: "uva_code");
                });

            migrationBuilder.CreateTable(
                name: "vol_program_patterns",
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
                    table.PrimaryKey("vol_program_patterns_pkey", x => x.id);
                    table.UniqueConstraint("AK_vol_program_patterns_uva_code", x => x.uva_code);
                    table.ForeignKey(
                        name: "vol_program_patterns_program_id_fkey",
                        column: x => x.program_code,
                        principalTable: "vol_programs",
                        principalColumn: "uva_code");
                });

            migrationBuilder.CreateTable(
                name: "vol_program_pattern_details",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uva_code = table.Column<string>(type: "text", nullable: false),
                    vol_program_pattern_code = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("vol_program_pattern_details_pkey", x => x.id);
                    table.ForeignKey(
                        name: "vol_program_pattern_details_vol_program_pattern_id_fkey",
                        column: x => x.vol_program_pattern_code,
                        principalTable: "vol_program_patterns",
                        principalColumn: "uva_code");
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_security_audits_profile_code",
                table: "user_security_audits",
                column: "profile_code");

            migrationBuilder.CreateIndex(
                name: "IX_user_security_audits_profile_code_device_fingerprint",
                table: "user_security_audits",
                columns: new[] { "profile_code", "device_fingerprint" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_security_audits_uva_code",
                table: "user_security_audits",
                column: "uva_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vol_program_collaborators_assigned_by_profile_code",
                table: "vol_program_collaborators",
                column: "assigned_by_profile_code");

            migrationBuilder.CreateIndex(
                name: "IX_vol_program_collaborators_profile_code",
                table: "vol_program_collaborators",
                column: "profile_code");

            migrationBuilder.CreateIndex(
                name: "IX_vol_program_collaborators_program_code",
                table: "vol_program_collaborators",
                column: "program_code");

            migrationBuilder.CreateIndex(
                name: "IX_vol_program_collaborators_uva_code",
                table: "vol_program_collaborators",
                column: "uva_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vol_program_contents_uva_code",
                table: "vol_program_contents",
                column: "uva_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "program_content_program_id_key",
                table: "vol_program_contents",
                column: "program_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vol_program_pattern_details_uva_code",
                table: "vol_program_pattern_details",
                column: "uva_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vol_program_pattern_details_vol_program_pattern_code",
                table: "vol_program_pattern_details",
                column: "vol_program_pattern_code");

            migrationBuilder.CreateIndex(
                name: "IX_vol_program_patterns_program_code",
                table: "vol_program_patterns",
                column: "program_code");

            migrationBuilder.CreateIndex(
                name: "IX_vol_program_patterns_uva_code",
                table: "vol_program_patterns",
                column: "uva_code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "activities_vol_program_pattern_id_fkey",
                table: "activities",
                column: "vol_program_pattern_code",
                principalTable: "vol_program_patterns",
                principalColumn: "uva_code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "activities_vol_program_pattern_id_fkey",
                table: "activities");

            migrationBuilder.DropTable(
                name: "user_security_audits");

            migrationBuilder.DropTable(
                name: "vol_program_collaborators");

            migrationBuilder.DropTable(
                name: "vol_program_contents");

            migrationBuilder.DropTable(
                name: "vol_program_pattern_details");

            migrationBuilder.DropTable(
                name: "vol_program_patterns");

            migrationBuilder.RenameColumn(
                name: "vol_program_pattern_code",
                table: "activities",
                newName: "activity_recurrence_pattern_code");

            migrationBuilder.RenameIndex(
                name: "IX_activities_vol_program_pattern_code",
                table: "activities",
                newName: "IX_activities_activity_recurrence_pattern_code");

            migrationBuilder.CreateTable(
                name: "activity_recurrence_patterns",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    program_code = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    recurrence_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    state_code = table.Column<string>(type: "text", nullable: false, defaultValue: "stage-1"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    uva_code = table.Column<string>(type: "text", nullable: false)
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
                    assigned_by_profile_code = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    profile_code = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    program_code = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    state_code = table.Column<string>(type: "text", nullable: false, defaultValue: "stage-1"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    uva_code = table.Column<string>(type: "text", nullable: false)
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
                    program_code = table.Column<string>(type: "text", nullable: false),
                    activities_description = table.Column<string>(type: "text", nullable: true),
                    contact_info = table.Column<string>(type: "text", nullable: true),
                    cover_photo_url = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    description = table.Column<string>(type: "text", nullable: true),
                    leadership_info = table.Column<string>(type: "text", nullable: true),
                    mission_statement = table.Column<string>(type: "text", nullable: true),
                    profile_photo_url = table.Column<string>(type: "text", nullable: true),
                    schedule_info = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    uva_code = table.Column<string>(type: "text", nullable: false)
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
                name: "activity_recurrence_detail",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    activity_recurrence_pattern_code = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    day_of_month = table.Column<short>(type: "smallint", nullable: true),
                    day_of_week = table.Column<short>(type: "smallint", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    end_hour = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    start_hour = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    state_code = table.Column<string>(type: "text", nullable: false, defaultValue: "stage-1"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    uva_code = table.Column<string>(type: "text", nullable: false),
                    week_of_month = table.Column<short>(type: "smallint", nullable: true)
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

            migrationBuilder.AddForeignKey(
                name: "activities_activity_recurrence_pattern_id_fkey",
                table: "activities",
                column: "activity_recurrence_pattern_code",
                principalTable: "activity_recurrence_patterns",
                principalColumn: "uva_code");
        }
    }
}
