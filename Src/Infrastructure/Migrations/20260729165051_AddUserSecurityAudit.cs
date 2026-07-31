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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_security_audits");
        }
    }
}
