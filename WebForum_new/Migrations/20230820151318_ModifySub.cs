using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebForum_new.Migrations
{
    /// <inheritdoc />
    public partial class ModifySub : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunitySubscriptions_Communities_CommunityId",
                table: "CommunitySubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_CommunitySubscriptions_CommunityId",
                table: "CommunitySubscriptions");

            migrationBuilder.AlterColumn<int>(
                name: "CommunityId",
                table: "CommunitySubscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CommunityId",
                table: "CommunitySubscriptions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_CommunitySubscriptions_CommunityId",
                table: "CommunitySubscriptions",
                column: "CommunityId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunitySubscriptions_Communities_CommunityId",
                table: "CommunitySubscriptions",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id");
        }
    }
}
