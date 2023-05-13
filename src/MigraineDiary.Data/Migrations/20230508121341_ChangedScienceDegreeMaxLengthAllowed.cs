using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigraineDiary.Data.Migrations
{
    public partial class ChangedScienceDegreeMaxLengthAllowed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ScienceDegree",
                table: "Practicioners",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ScienceDegree",
                table: "Practicioners",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

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
    }
}
