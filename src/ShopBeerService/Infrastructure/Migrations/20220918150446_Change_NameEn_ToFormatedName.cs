using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShopBeerService.Migrations
{
    public partial class Change_NameEn_ToFormatedName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "ShopBeers");

          

            migrationBuilder.AddColumn<string>(
                name: "FormatedName",
                table: "ShopBeers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormatedName",
                table: "ShopBeers");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "ShopBeers",
                type: "text",
                nullable: true);
        }
    }
}
