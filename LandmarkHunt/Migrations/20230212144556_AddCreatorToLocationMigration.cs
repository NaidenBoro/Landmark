using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LandmarkHunt.Migrations
{
    public partial class AddCreatorToLocationMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorUserId",
                table: "Locations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CreatorUserId",
                table: "Locations",
                column: "CreatorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_AspNetUsers_CreatorUserId",
                table: "Locations",
                column: "CreatorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_AspNetUsers_CreatorUserId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_CreatorUserId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Locations");
        }
    }
}
