using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebForum_new.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCreatedByIdToAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_AspNetUsers_AppUserId",
                table: "Communities");

            migrationBuilder.DropIndex(
                name: "IX_Communities_AppUserId",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Communities");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Communities",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Communities_CreatedById",
                table: "Communities",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_AspNetUsers_CreatedById",
                table: "Communities",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_AspNetUsers_CreatedById",
                table: "Communities");

            migrationBuilder.DropIndex(
                name: "IX_Communities_CreatedById",
                table: "Communities");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Communities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Communities",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Communities_AppUserId",
                table: "Communities",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_AspNetUsers_AppUserId",
                table: "Communities",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
