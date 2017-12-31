using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Tcust.Migrations
{
    public partial class _201712166 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partners_CountryAreas_CountryAreaId",
                table: "Partners");

            migrationBuilder.DropIndex(
                name: "IX_Partners_CountryAreaId",
                table: "Partners");

            migrationBuilder.RenameColumn(
                name: "CountryAreaId",
                table: "Partners",
                newName: "CountryId");

            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "Partners",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "Partners");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Partners",
                newName: "CountryAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_CountryAreaId",
                table: "Partners",
                column: "CountryAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_CountryAreas_CountryAreaId",
                table: "Partners",
                column: "CountryAreaId",
                principalTable: "CountryAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
