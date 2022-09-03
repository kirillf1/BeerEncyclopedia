using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShopBeerService.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopBeers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShopId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    Manufacturer = table.Column<string>(type: "text", nullable: true),
                    Volume = table.Column<double>(type: "double precision", nullable: true),
                    Brand = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    DiscountPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    Strength = table.Column<double>(type: "double precision", nullable: true),
                    Filtration = table.Column<bool>(type: "boolean", nullable: true),
                    Pasteurization = table.Column<bool>(type: "boolean", nullable: true),
                    DetailsUrl = table.Column<string>(type: "text", nullable: true),
                    Rating = table.Column<double>(type: "double precision", nullable: true),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    Style = table.Column<string>(type: "text", nullable: true),
                    InitialWort = table.Column<double>(type: "double precision", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SourceBeerId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopBeers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopBeers_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopBeers_Name",
                table: "ShopBeers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ShopBeers_ShopId",
                table: "ShopBeers",
                column: "ShopId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopBeers");

            migrationBuilder.DropTable(
                name: "Shops");
        }
    }
}
