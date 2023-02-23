using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LandmarkHunt.Migrations
{
    public partial class AddedPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoId",
                table: "Locations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Bytes = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_PhotoId",
                table: "Locations",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Photos_PhotoId",
                table: "Locations",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Photos_PhotoId",
                table: "Locations");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Locations_PhotoId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Locations");
        }
    }
}
