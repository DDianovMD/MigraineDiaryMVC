using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigraineDiary.Data.Migrations
{
    public partial class PracticionerAuditableAndSoftDeletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Practicioners",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Practicioners",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Practicioners",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d75eec2-411b-43d0-acb7-5ae4bf74555f",
                column: "ConcurrencyStamp",
                value: "d6671852-1e94-4cd8-b552-60d087df128a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff3d52a7-7288-42aa-9955-6c4c4ad4caed",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c14baa9f-ca6e-4d5a-bea7-f5c0913332b3", "AQAAAAEAACcQAAAAEH9prb9jVYCpVWzVpeif4qHj6crcRXBXQNbyY6yYsnIrV49cO/hGVKRuh0TVg9Z+kg==", "da93050b-c7c9-48ea-b4e3-fda9e0688857" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Practicioners");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Practicioners");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Practicioners");

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
    }
}
