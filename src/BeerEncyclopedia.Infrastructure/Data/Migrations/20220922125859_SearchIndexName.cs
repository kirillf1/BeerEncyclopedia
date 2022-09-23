using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeerEncyclopedia.Infrastructure.Migrations
{
    public partial class SearchIndexName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Manufacturers_Name",
                table: "Manufacturers");

            migrationBuilder.DropIndex(
                name: "IX_Beers_AltName",
                table: "Beers");

            migrationBuilder.DropIndex(
                name: "IX_Beers_Name",
                table: "Beers");

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_Name",
                table: "Manufacturers",
                column: "Name")
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "russian");

            migrationBuilder.CreateIndex(
                name: "IX_Beers_Name_AltName",
                table: "Beers",
                columns: new[] { "Name", "AltName" })
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "russian");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Manufacturers_Name",
                table: "Manufacturers");

            migrationBuilder.DropIndex(
                name: "IX_Beers_Name_AltName",
                table: "Beers");

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_Name",
                table: "Manufacturers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Beers_AltName",
                table: "Beers",
                column: "AltName");

            migrationBuilder.CreateIndex(
                name: "IX_Beers_Name",
                table: "Beers",
                column: "Name");
        }
    }
}
