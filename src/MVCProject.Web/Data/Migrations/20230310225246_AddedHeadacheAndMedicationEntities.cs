using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigraineDiary.Web.Data.Migrations
{
    public partial class AddedHeadacheAndMedicationEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "AspNetUsers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Headache",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Onset = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HeadacheDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    LocalizationSide = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    PainCharacteristics = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Photophoby = table.Column<bool>(type: "bit", nullable: false),
                    Phonophoby = table.Column<bool>(type: "bit", nullable: false),
                    Nausea = table.Column<bool>(type: "bit", nullable: false),
                    Vomiting = table.Column<bool>(type: "bit", nullable: false),
                    Aura = table.Column<bool>(type: "bit", nullable: false),
                    AuraDescriptionNotes = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    Triggers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Headache", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Headache_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Medication",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GenericName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SinglePillDosage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Units = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    NumberOfTakenPills = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medication", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserHeadache",
                columns: table => new
                {
                    SharedWithId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SharedWithMeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserHeadache", x => new { x.SharedWithId, x.SharedWithMeId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserHeadache_AspNetUsers_SharedWithId",
                        column: x => x.SharedWithId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserHeadache_Headache_SharedWithMeId",
                        column: x => x.SharedWithMeId,
                        principalTable: "Headache",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HeadacheMedication",
                columns: table => new
                {
                    HeadachesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MedicationsTakenId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeadacheMedication", x => new { x.HeadachesId, x.MedicationsTakenId });
                    table.ForeignKey(
                        name: "FK_HeadacheMedication_Headache_HeadachesId",
                        column: x => x.HeadachesId,
                        principalTable: "Headache",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeadacheMedication_Medication_MedicationsTakenId",
                        column: x => x.MedicationsTakenId,
                        principalTable: "Medication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserHeadache_SharedWithMeId",
                table: "ApplicationUserHeadache",
                column: "SharedWithMeId");

            migrationBuilder.CreateIndex(
                name: "IX_Headache_PatientId",
                table: "Headache",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_HeadacheMedication_MedicationsTakenId",
                table: "HeadacheMedication",
                column: "MedicationsTakenId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserHeadache");

            migrationBuilder.DropTable(
                name: "HeadacheMedication");

            migrationBuilder.DropTable(
                name: "Headache");

            migrationBuilder.DropTable(
                name: "Medication");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "AspNetUsers");
        }
    }
}
