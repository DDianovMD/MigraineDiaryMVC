using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigraineDiary.Data.Migrations
{
    public partial class ChangedHeadacheMedicationRelationType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeadacheMedication");

            migrationBuilder.AddColumn<string>(
                name: "HeadacheId",
                table: "Medications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_HeadacheId",
                table: "Medications",
                column: "HeadacheId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_Headaches_HeadacheId",
                table: "Medications",
                column: "HeadacheId",
                principalTable: "Headaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medications_Headaches_HeadacheId",
                table: "Medications");

            migrationBuilder.DropIndex(
                name: "IX_Medications_HeadacheId",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "HeadacheId",
                table: "Medications");

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
                        name: "FK_HeadacheMedication_Headaches_HeadachesId",
                        column: x => x.HeadachesId,
                        principalTable: "Headaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeadacheMedication_Medications_MedicationsTakenId",
                        column: x => x.MedicationsTakenId,
                        principalTable: "Medications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HeadacheMedication_MedicationsTakenId",
                table: "HeadacheMedication",
                column: "MedicationsTakenId");
        }
    }
}
