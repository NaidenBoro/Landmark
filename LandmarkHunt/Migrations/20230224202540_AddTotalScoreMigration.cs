using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LandmarkHunt.Migrations
{
    public partial class AddTotalScoreMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionChallenges_Challenges_ChallengeId",
                table: "SessionChallenges");

            migrationBuilder.AddColumn<int>(
                name: "TotalScore",
                table: "SessionChallenges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionChallenges_Challenges_ChallengeId",
                table: "SessionChallenges",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionChallenges_Challenges_ChallengeId",
                table: "SessionChallenges");

            migrationBuilder.DropColumn(
                name: "TotalScore",
                table: "SessionChallenges");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionChallenges_Challenges_ChallengeId",
                table: "SessionChallenges",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
