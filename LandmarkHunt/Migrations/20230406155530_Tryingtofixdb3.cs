using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LandmarkHunt.Migrations
{
    public partial class Tryingtofixdb3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeLocations_Challenges_ChallengeId",
                table: "ChallengeLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeLocations_Locations_LocationId",
                table: "ChallengeLocations");

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeLocations_Challenges_ChallengeId",
                table: "ChallengeLocations",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeLocations_Locations_LocationId",
                table: "ChallengeLocations",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeLocations_Challenges_ChallengeId",
                table: "ChallengeLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeLocations_Locations_LocationId",
                table: "ChallengeLocations");

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeLocations_Challenges_ChallengeId",
                table: "ChallengeLocations",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeLocations_Locations_LocationId",
                table: "ChallengeLocations",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
