using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce_Admin_Dashboard.Migrations
{
    /// <inheritdoc />
    public partial class AdjustTableFieldsForLogicalClarity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminActions_Customers_TargetUserId",
                table: "AdminActions");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "PurchaseItems");

            migrationBuilder.DropColumn(
                name: "BannedDays",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsWarned",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "WarningLevel",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "TargetUserId",
                table: "AdminActions",
                newName: "TargetCustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_AdminActions_TargetUserId",
                table: "AdminActions",
                newName: "IX_AdminActions_TargetCustomerId");

            migrationBuilder.AddColumn<int>(
                name: "BannedDays",
                table: "Users",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWarned",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "WarningLevel",
                table: "Users",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "AdminActions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdminActions_CustomerId",
                table: "AdminActions",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminActions_Customers_CustomerId",
                table: "AdminActions",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminActions_Users_TargetCustomerId",
                table: "AdminActions",
                column: "TargetCustomerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminActions_Customers_CustomerId",
                table: "AdminActions");

            migrationBuilder.DropForeignKey(
                name: "FK_AdminActions_Users_TargetCustomerId",
                table: "AdminActions");

            migrationBuilder.DropIndex(
                name: "IX_AdminActions_CustomerId",
                table: "AdminActions");

            migrationBuilder.DropColumn(
                name: "BannedDays",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsWarned",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WarningLevel",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "AdminActions");

            migrationBuilder.RenameColumn(
                name: "TargetCustomerId",
                table: "AdminActions",
                newName: "TargetUserId");

            migrationBuilder.RenameIndex(
                name: "IX_AdminActions_TargetCustomerId",
                table: "AdminActions",
                newName: "IX_AdminActions_TargetUserId");

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "PurchaseItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "BannedDays",
                table: "Customers",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWarned",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "WarningLevel",
                table: "Customers",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_AdminActions_Customers_TargetUserId",
                table: "AdminActions",
                column: "TargetUserId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
