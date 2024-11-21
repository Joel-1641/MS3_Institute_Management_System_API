using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MSS1.Migrations
{
    /// <inheritdoc />
    public partial class Test20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRegistrationFeePaid",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsForRegistrationFee",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRegistrationFeePaid",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsForRegistrationFee",
                table: "Payments");
        }
    }
}
