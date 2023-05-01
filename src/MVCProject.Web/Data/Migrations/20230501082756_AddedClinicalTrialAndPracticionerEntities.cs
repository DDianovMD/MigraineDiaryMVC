using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigraineDiary.Web.Migrations
{
    public partial class AddedClinicalTrialAndPracticionerEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClinicalTrials",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hospital = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicalTrials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicalTrials_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Practicioners",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScienceDegree = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ClinicalTrialId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Practicioners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Practicioners_ClinicalTrials_ClinicalTrialId",
                        column: x => x.ClinicalTrialId,
                        principalTable: "ClinicalTrials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d75eec2-411b-43d0-acb7-5ae4bf74555f",
                column: "ConcurrencyStamp",
                value: "79e0befc-1f3e-4027-9d9f-0cfbcab9766c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff3d52a7-7288-42aa-9955-6c4c4ad4caed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ec5cdf78-2dad-495d-bcc0-c22587ea2dff", "AQAAAAEAACcQAAAAEL0RTWVf11igTsK45EjyrcavgcpXklySsdlnLFyP4IgVV2ztX26un9wg46W/xhgodw==", "2856e724-9c8b-488d-a006-ff48ec24ef17" });

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalTrials_CreatorId",
                table: "ClinicalTrials",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Practicioners_ClinicalTrialId",
                table: "Practicioners",
                column: "ClinicalTrialId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Practicioners");

            migrationBuilder.DropTable(
                name: "ClinicalTrials");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d75eec2-411b-43d0-acb7-5ae4bf74555f",
                column: "ConcurrencyStamp",
                value: "2467815b-6717-41b8-a63f-8035a53319b2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff3d52a7-7288-42aa-9955-6c4c4ad4caed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "58771f9a-a435-468e-a277-ae724715a7ff", "AQAAAAEAACcQAAAAEHd+kaoA/avB/Wvg4RyEU9LGHdswzRNHe/LH3A7V7X+6IoIxMAvdQZGa/uhpqE+v4Q==", "32d37869-c919-46a6-8c7b-1f56ca07c81d" });
        }
    }
}
