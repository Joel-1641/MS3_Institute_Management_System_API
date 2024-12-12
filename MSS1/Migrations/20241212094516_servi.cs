using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MSS1.Migrations
{
    /// <inheritdoc />
    public partial class servi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Notifications",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Notifications",
                newName: "Target");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Notifications",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                newName: "IX_Notifications_StudentId");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "NotificationType",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Students_StudentId",
                table: "Notifications",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Students_StudentId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "NotificationType",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "Target",
                table: "Notifications",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Notifications",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Notifications",
                newName: "CreatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_StudentId",
                table: "Notifications",
                newName: "IX_Notifications_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
