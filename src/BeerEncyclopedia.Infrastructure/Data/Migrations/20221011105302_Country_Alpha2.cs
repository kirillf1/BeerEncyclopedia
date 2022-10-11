using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeerEncyclopedia.Infrastructure.Migrations
{
    public partial class Country_Alpha2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         
            migrationBuilder.AddColumn<string>(
                name: "Alpha2",
                table: "Countries",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropColumn(
                name: "Alpha2",
                table: "Countries");
        }
    }
}
