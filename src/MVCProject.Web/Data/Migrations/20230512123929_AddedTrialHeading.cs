using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigraineDiary.Web.Migrations
{
    public partial class AddedTrialHeading : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Heading",
                table: "ClinicalTrials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d75eec2-411b-43d0-acb7-5ae4bf74555f",
                column: "ConcurrencyStamp",
                value: "64a74fbd-ac94-4822-81f3-59c0d168ac1d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff3d52a7-7288-42aa-9955-6c4c4ad4caed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2ccd5555-e12d-43e1-8372-ea989250ac0c", "AQAAAAEAACcQAAAAEJlzejG9IDbX/SeQFn2Yek7aYtSpOLImAJLaoHfWbf60irVCQDH/I7GbbhyRkec7rw==", "8f0a2e19-5ca7-4d83-aed2-e41d8323abf8" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Heading",
                table: "ClinicalTrials");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d75eec2-411b-43d0-acb7-5ae4bf74555f",
                column: "ConcurrencyStamp",
                value: "183b762a-29f3-467a-b560-8e155c61345b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff3d52a7-7288-42aa-9955-6c4c4ad4caed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f6959678-9783-48a7-8cf0-8ae8e1a5d562", "AQAAAAEAACcQAAAAEC9cFJP16NRA9uThKZr16zU32PZF8auv3zW3EZ/iT1Autl+7GxR/oE2HbVnXDJ+0UA==", "207d5b83-dbda-4174-b6b4-e82220f69c27" });
        }
    }
}
