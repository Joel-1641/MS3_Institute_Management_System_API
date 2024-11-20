using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MSS1.Migrations
{
    /// <inheritdoc />
    public partial class teast8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authenticators_Users_UserId",
                table: "Authenticators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Authenticators",
                table: "Authenticators");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Admins");

            migrationBuilder.RenameTable(
                name: "Authenticators",
                newName: "Authentications");

            migrationBuilder.RenameIndex(
                name: "IX_Authenticators_UserId",
                table: "Authentications",
                newName: "IX_Authentications_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Authentications",
                table: "Authentications",
                column: "AuthenticationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authentications_Users_UserId",
                table: "Authentications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authentications_Users_UserId",
                table: "Authentications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Authentications",
                table: "Authentications");

            migrationBuilder.RenameTable(
                name: "Authentications",
                newName: "Authenticators");

            migrationBuilder.RenameIndex(
                name: "IX_Authentications_UserId",
                table: "Authenticators",
                newName: "IX_Authenticators_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Authenticators",
                table: "Authenticators",
                column: "AuthenticationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authenticators_Users_UserId",
                table: "Authenticators",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
