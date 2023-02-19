using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LandmarkHunt.Migrations
{
    public partial class AddChallengeMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChallengeId",
                table: "Locations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ChallengeId",
                table: "Locations",
                column: "ChallengeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Challenges_ChallengeId",
                table: "Locations",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Challenges_ChallengeId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_ChallengeId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ChallengeId",
                table: "Locations");
        }
    }
}
