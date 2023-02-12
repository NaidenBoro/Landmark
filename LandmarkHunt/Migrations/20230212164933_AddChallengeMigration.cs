using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LandmarkHunt.Migrations
{
    public partial class AddChallengeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Challenges",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Challenges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Challenges_AspNetUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeLocations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChallengeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocationId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengeLocations_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeLocations_ChallengeId",
                table: "ChallengeLocations",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeLocations_LocationId",
                table: "ChallengeLocations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_CreatorUserId",
                table: "Challenges",
                column: "CreatorUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallengeLocations");

            migrationBuilder.DropTable(
                name: "Challenges");
        }
    }
}
