using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LandmarkHunt.Migrations
{
    public partial class AddChallenge2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_SessionChallenges_SessionChallengeId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_SessionChallengeId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "SessionChallengeId",
                table: "Locations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SessionChallengeId",
                table: "Locations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_SessionChallengeId",
                table: "Locations",
                column: "SessionChallengeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_SessionChallenges_SessionChallengeId",
                table: "Locations",
                column: "SessionChallengeId",
                principalTable: "SessionChallenges",
                principalColumn: "Id");
        }
    }
}
