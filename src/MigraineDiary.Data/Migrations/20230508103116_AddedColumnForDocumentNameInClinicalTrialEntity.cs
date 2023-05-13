using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigraineDiary.Data.Migrations
{
    public partial class AddedColumnForDocumentNameInClinicalTrialEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AgreementDocumentName",
                table: "ClinicalTrials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d75eec2-411b-43d0-acb7-5ae4bf74555f",
                column: "ConcurrencyStamp",
                value: "c2374d07-e421-463b-a166-b6332701d490");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff3d52a7-7288-42aa-9955-6c4c4ad4caed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5ef1bb46-8cde-41dd-95fa-3c29885898cc", "AQAAAAEAACcQAAAAEB60C4kK2CTbgGZwM1q4wynfMyKAGYJoRKdqjmBBZR+UIDqe4S5uH0+ILetws+YI0A==", "23245063-2d31-458f-a310-b03a5b01e4d8" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgreementDocumentName",
                table: "ClinicalTrials");

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
        }
    }
}
