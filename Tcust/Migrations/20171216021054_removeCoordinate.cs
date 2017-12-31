using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Tcust.Migrations
{
    public partial class removeCoordinate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CountryAreas_Coordinates_CoordinateId",
                table: "CountryAreas");

            migrationBuilder.DropTable(
                name: "Coordinates");

            migrationBuilder.DropIndex(
                name: "IX_CountryAreas_CoordinateId",
                table: "CountryAreas");

            migrationBuilder.DropColumn(
                name: "CoordinateId",
                table: "CountryAreas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoordinateId",
                table: "CountryAreas",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Coordinates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PointX = table.Column<string>(nullable: true),
                    PointY = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountryAreas_CoordinateId",
                table: "CountryAreas",
                column: "CoordinateId");

            migrationBuilder.AddForeignKey(
                name: "FK_CountryAreas_Coordinates_CoordinateId",
                table: "CountryAreas",
                column: "CoordinateId",
                principalTable: "Coordinates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
