using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LandmarkHunt.Migrations
{
    public partial class AddChallenge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SessionChallengeId",
                table: "Locations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SessionChallenges",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChallengeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionChallenges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionChallenges_AspNetUsers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SessionChallenges_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_SessionChallengeId",
                table: "Locations",
                column: "SessionChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionChallenges_ChallengeId",
                table: "SessionChallenges",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionChallenges_PlayerId",
                table: "SessionChallenges",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_SessionChallenges_SessionChallengeId",
                table: "Locations",
                column: "SessionChallengeId",
                principalTable: "SessionChallenges",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_SessionChallenges_SessionChallengeId",
                table: "Locations");

            migrationBuilder.DropTable(
                name: "SessionChallenges");

            migrationBuilder.DropIndex(
                name: "IX_Locations_SessionChallengeId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "SessionChallengeId",
                table: "Locations");
        }
    }
}
