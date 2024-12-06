using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MSS1.Migrations
{
    /// <inheritdoc />
    public partial class Notify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Courses",
                newName: "CourseDuration");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseDuration",
                table: "Courses",
                newName: "Duration");
        }
    }
}
