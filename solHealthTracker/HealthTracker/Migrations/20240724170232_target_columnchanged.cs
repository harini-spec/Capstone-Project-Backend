﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthTracker.Migrations
{
    public partial class target_columnchanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Targets");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Targets",
                newName: "TargetDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TargetDate",
                table: "Targets",
                newName: "StartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Targets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
