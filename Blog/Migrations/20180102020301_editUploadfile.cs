using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Blog.Migrations
{
    public partial class editUploadfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemOID",
                table: "UploadFiles");

            migrationBuilder.DropColumn(
                name: "Top",
                table: "UploadFiles");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "UploadFiles",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "UploadFiles");

            migrationBuilder.AddColumn<string>(
                name: "ItemOID",
                table: "UploadFiles",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Top",
                table: "UploadFiles",
                nullable: false,
                defaultValue: false);
        }
    }
}
