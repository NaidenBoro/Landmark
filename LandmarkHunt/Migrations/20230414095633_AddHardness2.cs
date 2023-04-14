using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LandmarkHunt.Migrations
{
    public partial class AddHardness2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hardness",
                table: "UserGuesses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Easy");

            migrationBuilder.AddColumn<string>(
                name: "Hardness",
                table: "SessionChallenges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Easy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hardness",
                table: "UserGuesses");

            migrationBuilder.DropColumn(
                name: "Hardness",
                table: "SessionChallenges");
        }
    }
}
