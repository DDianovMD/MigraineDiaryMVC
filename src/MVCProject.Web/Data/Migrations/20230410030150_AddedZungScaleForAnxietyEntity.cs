using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigraineDiary.Web.Migrations
{
    public partial class AddedZungScaleForAnxietyEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZungScalesForAnxiety",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SecondQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ThirdQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FourthQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FifthQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SixthQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SeventhQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EighthQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NinthQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenthQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EleventhQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TwelfthQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ThirteenthQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FourteenthQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FifteenthQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SixteenthQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SeventeenthQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EighteenthQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NineteenthQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TwentiethQuestionAnswer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZungScalesForAnxiety", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZungScalesForAnxiety_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserZungScaleForAnxiety",
                columns: table => new
                {
                    SharedWithId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SharedZungScalesForAnxietyWithMeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserZungScaleForAnxiety", x => new { x.SharedWithId, x.SharedZungScalesForAnxietyWithMeId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserZungScaleForAnxiety_AspNetUsers_SharedWithId",
                        column: x => x.SharedWithId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserZungScaleForAnxiety_ZungScalesForAnxiety_SharedZungScalesForAnxietyWithMeId",
                        column: x => x.SharedZungScalesForAnxietyWithMeId,
                        principalTable: "ZungScalesForAnxiety",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserZungScaleForAnxiety_SharedZungScalesForAnxietyWithMeId",
                table: "ApplicationUserZungScaleForAnxiety",
                column: "SharedZungScalesForAnxietyWithMeId");

            migrationBuilder.CreateIndex(
                name: "IX_ZungScalesForAnxiety_PatientId",
                table: "ZungScalesForAnxiety",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserZungScaleForAnxiety");

            migrationBuilder.DropTable(
                name: "ZungScalesForAnxiety");
        }
    }
}
