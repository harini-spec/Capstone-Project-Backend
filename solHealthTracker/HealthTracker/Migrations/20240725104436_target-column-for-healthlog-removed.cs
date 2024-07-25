using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthTracker.Migrations
{
    public partial class targetcolumnforhealthlogremoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthLogs_Targets_TargetId",
                table: "HealthLogs");

            migrationBuilder.DropIndex(
                name: "IX_HealthLogs_TargetId",
                table: "HealthLogs");

            migrationBuilder.DropColumn(
                name: "TargetId",
                table: "HealthLogs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TargetId",
                table: "HealthLogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HealthLogs_TargetId",
                table: "HealthLogs",
                column: "TargetId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthLogs_Targets_TargetId",
                table: "HealthLogs",
                column: "TargetId",
                principalTable: "Targets",
                principalColumn: "Id");
        }
    }
}
