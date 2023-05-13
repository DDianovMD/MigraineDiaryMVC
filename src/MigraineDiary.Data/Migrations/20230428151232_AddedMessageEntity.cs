using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigraineDiary.Data.Migrations
{
    public partial class AddedMessageEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SenderName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    SenderEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MessageContent = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d75eec2-411b-43d0-acb7-5ae4bf74555f",
                column: "ConcurrencyStamp",
                value: "cd8ca75f-986e-436b-8c30-e5ca0f5e442b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff3d52a7-7288-42aa-9955-6c4c4ad4caed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "78a9b6c3-cf45-430a-b802-1d8708539e58", "AQAAAAEAACcQAAAAEMr0pqBorozrVX0E99vmkmN1+b4w5ViyXMUOG/zp1OAzjG64RiDjawxAzOV+WIFzBw==", "e416082d-93fc-44ec-b0af-889afd24a303" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d75eec2-411b-43d0-acb7-5ae4bf74555f",
                column: "ConcurrencyStamp",
                value: "e1104912-2b0a-4ef5-bc26-7b2294566b49");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff3d52a7-7288-42aa-9955-6c4c4ad4caed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8ab493c4-ac65-4a6f-8d9d-6c20af83f84d", "AQAAAAEAACcQAAAAEGWqMfHFGB9DxF1EOon6QbHh++rK4iKhDeZxISIV9Zn4gvAdeik8TDksoBZXcBVahQ==", "fa49c457-d02e-47c6-b2a6-f7c2b41cc194" });
        }
    }
}
