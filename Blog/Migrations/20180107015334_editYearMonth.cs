using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Blog.Migrations
{
    public partial class editYearMonth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateYear",
                table: "Posts",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "CreateMonth",
                table: "Posts",
                newName: "Month");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Posts",
                newName: "CreateYear");

            migrationBuilder.RenameColumn(
                name: "Month",
                table: "Posts",
                newName: "CreateMonth");
        }
    }
}
