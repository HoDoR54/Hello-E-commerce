using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce_Admin_Dashboard.Migrations
{
    /// <inheritdoc />
    public partial class SeedSuperAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Admins");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "IsSuperAdmin", "Name", "Password", "PhoneNumber", "UpdatedAt" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 5, 21, 12, 0, 0, 0, DateTimeKind.Utc), null, "hponetaukyou@gmail.com", true, "Hpone Tauk Nyi", "$2a$12$90UrUp1k5/Zmzx9b3Ms8YunIR5.zexGCRLj3G/ztUVzFUpQpFAC7.", "+959890079614", new DateTime(2025, 5, 21, 12, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Admins",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
