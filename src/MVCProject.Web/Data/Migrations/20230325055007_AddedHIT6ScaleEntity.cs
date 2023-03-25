using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigraineDiary.Web.Migrations
{
    public partial class AddedHIT6ScaleEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HIT6Scales",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstQuestionAnswer = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SecondQuestionAnswer = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ThirdQuestionAnswer = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FourthQuestionAnswer = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FifthQuestionAnswer = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SixthQuestionAnswer = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HIT6Scales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HIT6Scales_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserHIT6Scale",
                columns: table => new
                {
                    SharedHIT6ScalesWithMeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SharedWithId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserHIT6Scale", x => new { x.SharedHIT6ScalesWithMeId, x.SharedWithId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserHIT6Scale_AspNetUsers_SharedWithId",
                        column: x => x.SharedWithId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserHIT6Scale_HIT6Scales_SharedHIT6ScalesWithMeId",
                        column: x => x.SharedHIT6ScalesWithMeId,
                        principalTable: "HIT6Scales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserHIT6Scale_SharedWithId",
                table: "ApplicationUserHIT6Scale",
                column: "SharedWithId");

            migrationBuilder.CreateIndex(
                name: "IX_HIT6Scales_PatientId",
                table: "HIT6Scales",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserHIT6Scale");

            migrationBuilder.DropTable(
                name: "HIT6Scales");
        }
    }
}
