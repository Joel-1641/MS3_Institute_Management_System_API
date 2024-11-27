using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MSS1.Migrations
{
    /// <inheritdoc />
    public partial class Thushyan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoginDate",
                table: "Admins");

            migrationBuilder.RenameColumn(
                name: "AdminRoleType",
                table: "Admins",
                newName: "fullName");

            migrationBuilder.RenameColumn(
                name: "AdminPhoneNumber",
                table: "Admins",
                newName: "Email");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Admins",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Admins");

            migrationBuilder.RenameColumn(
                name: "fullName",
                table: "Admins",
                newName: "AdminRoleType");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Admins",
                newName: "AdminPhoneNumber");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDate",
                table: "Admins",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
