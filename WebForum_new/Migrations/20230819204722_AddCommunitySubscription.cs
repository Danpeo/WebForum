using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebForum_new.Migrations
{
    /// <inheritdoc />
    public partial class AddCommunitySubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommunitySubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommunityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunitySubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunitySubscriptions_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunitySubscriptions_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunitySubscriptions_AppUserId",
                table: "CommunitySubscriptions",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunitySubscriptions_CommunityId",
                table: "CommunitySubscriptions",
                column: "CommunityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunitySubscriptions");
        }
    }
}
