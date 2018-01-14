using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Blog.Migrations
{
    public partial class editAppUser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermissionId",
                table: "AppUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PermissionId",
                table: "AppUsers",
                nullable: false,
                defaultValue: 0);
        }
    }
}
