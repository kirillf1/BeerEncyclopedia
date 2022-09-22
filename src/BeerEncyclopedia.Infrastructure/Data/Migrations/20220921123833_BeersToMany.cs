using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeerEncyclopedia.Infrastructure.Migrations
{
    public partial class BeersToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Manufacturers_Beers_BeerId",
                table: "Manufacturers");

            migrationBuilder.DropForeignKey(
                name: "FK_Styles_Beers_BeerId",
                table: "Styles");

            migrationBuilder.DropIndex(
                name: "IX_Styles_BeerId",
                table: "Styles");

            migrationBuilder.DropIndex(
                name: "IX_Manufacturers_BeerId",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "BeerId",
                table: "Styles");

            migrationBuilder.DropColumn(
                name: "BeerId",
                table: "Manufacturers");

            migrationBuilder.AddColumn<Guid>(
                name: "ColorId",
                table: "Beers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BeerManufacturer",
                columns: table => new
                {
                    BeersId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManufacturersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeerManufacturer", x => new { x.BeersId, x.ManufacturersId });
                    table.ForeignKey(
                        name: "FK_BeerManufacturer_Beers_BeersId",
                        column: x => x.BeersId,
                        principalTable: "Beers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BeerManufacturer_Manufacturers_ManufacturersId",
                        column: x => x.ManufacturersId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BeerStyle",
                columns: table => new
                {
                    BeersId = table.Column<Guid>(type: "uuid", nullable: false),
                    StylesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeerStyle", x => new { x.BeersId, x.StylesId });
                    table.ForeignKey(
                        name: "FK_BeerStyle_Beers_BeersId",
                        column: x => x.BeersId,
                        principalTable: "Beers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BeerStyle_Styles_StylesId",
                        column: x => x.StylesId,
                        principalTable: "Styles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Beers_ColorId",
                table: "Beers",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_BeerManufacturer_ManufacturersId",
                table: "BeerManufacturer",
                column: "ManufacturersId");

            migrationBuilder.CreateIndex(
                name: "IX_BeerStyle_StylesId",
                table: "BeerStyle",
                column: "StylesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Beers_Colors_ColorId",
                table: "Beers",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beers_Colors_ColorId",
                table: "Beers");

            migrationBuilder.DropTable(
                name: "BeerManufacturer");

            migrationBuilder.DropTable(
                name: "BeerStyle");

            migrationBuilder.DropIndex(
                name: "IX_Beers_ColorId",
                table: "Beers");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "Beers");

            migrationBuilder.AddColumn<Guid>(
                name: "BeerId",
                table: "Styles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BeerId",
                table: "Manufacturers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Styles_BeerId",
                table: "Styles",
                column: "BeerId");

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_BeerId",
                table: "Manufacturers",
                column: "BeerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Manufacturers_Beers_BeerId",
                table: "Manufacturers",
                column: "BeerId",
                principalTable: "Beers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Styles_Beers_BeerId",
                table: "Styles",
                column: "BeerId",
                principalTable: "Beers",
                principalColumn: "Id");
        }
    }
}
