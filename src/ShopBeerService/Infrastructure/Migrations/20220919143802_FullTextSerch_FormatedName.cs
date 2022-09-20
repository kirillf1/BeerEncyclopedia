using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopBeerService.Migrations
{
    public partial class FullTextSerch_FormatedName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ShopBeers_FormatedName",
                table: "ShopBeers",
                column: "FormatedName")
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "english");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShopBeers_FormatedName",
                table: "ShopBeers");
        }
    }
}
