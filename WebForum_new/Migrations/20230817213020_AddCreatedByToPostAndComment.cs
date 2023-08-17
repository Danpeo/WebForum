using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebForum_new.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByToPostAndComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Comments");
        }
    }
}
