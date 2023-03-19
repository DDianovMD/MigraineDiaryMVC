using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigraineDiary.Web.Migrations
{
    public partial class SplittedHeadacheDurationIntoSeparateProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeadacheDuration",
                table: "Headaches");

            migrationBuilder.AddColumn<int>(
                name: "DurationDays",
                table: "Headaches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DurationHours",
                table: "Headaches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Headaches",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationDays",
                table: "Headaches");

            migrationBuilder.DropColumn(
                name: "DurationHours",
                table: "Headaches");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Headaches");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HeadacheDuration",
                table: "Headaches",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
