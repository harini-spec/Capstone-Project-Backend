using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthTracker.Migrations
{
    public partial class column_added_preferencetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthLogs_UserPreferences_PreferenceId",
                table: "HealthLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Targets_UserPreferences_PreferenceId",
                table: "Targets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPreferences",
                table: "UserPreferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MonitorPreferences",
                table: "MonitorPreferences");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserPreferences",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MonitorPreferences",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPreferences",
                table: "UserPreferences",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonitorPreferences",
                table: "MonitorPreferences",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_UserId",
                table: "UserPreferences",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MonitorPreferences_CoachId",
                table: "MonitorPreferences",
                column: "CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthLogs_UserPreferences_PreferenceId",
                table: "HealthLogs",
                column: "PreferenceId",
                principalTable: "UserPreferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Targets_UserPreferences_PreferenceId",
                table: "Targets",
                column: "PreferenceId",
                principalTable: "UserPreferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthLogs_UserPreferences_PreferenceId",
                table: "HealthLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Targets_UserPreferences_PreferenceId",
                table: "Targets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPreferences",
                table: "UserPreferences");

            migrationBuilder.DropIndex(
                name: "IX_UserPreferences_UserId",
                table: "UserPreferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MonitorPreferences",
                table: "MonitorPreferences");

            migrationBuilder.DropIndex(
                name: "IX_MonitorPreferences_CoachId",
                table: "MonitorPreferences");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserPreferences");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MonitorPreferences");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPreferences",
                table: "UserPreferences",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonitorPreferences",
                table: "MonitorPreferences",
                column: "CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthLogs_UserPreferences_PreferenceId",
                table: "HealthLogs",
                column: "PreferenceId",
                principalTable: "UserPreferences",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Targets_UserPreferences_PreferenceId",
                table: "Targets",
                column: "PreferenceId",
                principalTable: "UserPreferences",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
