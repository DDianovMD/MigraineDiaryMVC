using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigraineDiary.Data.Migrations
{
    public partial class SeededAdminUserAndRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2d75eec2-411b-43d0-acb7-5ae4bf74555f", "e1104912-2b0a-4ef5-bc26-7b2294566b49", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsDeleted", "LastName", "LockoutEnabled", "LockoutEnd", "MiddleName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ff3d52a7-7288-42aa-9955-6c4c4ad4caed", 0, "8ab493c4-ac65-4a6f-8d9d-6c20af83f84d", "admin@migrainediary.com", true, null, false, null, true, null, null, "ADMIN@MIGRAINEDIARY.COM", "ADMIN", "AQAAAAEAACcQAAAAEGWqMfHFGB9DxF1EOon6QbHh++rK4iKhDeZxISIV9Zn4gvAdeik8TDksoBZXcBVahQ==", null, false, "fa49c457-d02e-47c6-b2a6-f7c2b41cc194", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "2d75eec2-411b-43d0-acb7-5ae4bf74555f", "ff3d52a7-7288-42aa-9955-6c4c4ad4caed" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2d75eec2-411b-43d0-acb7-5ae4bf74555f", "ff3d52a7-7288-42aa-9955-6c4c4ad4caed" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d75eec2-411b-43d0-acb7-5ae4bf74555f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff3d52a7-7288-42aa-9955-6c4c4ad4caed");
        }
    }
}
