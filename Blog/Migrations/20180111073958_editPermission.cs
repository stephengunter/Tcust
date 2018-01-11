using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Blog.Migrations
{
    public partial class editPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserPermissions",
                newName: "UserName");

            migrationBuilder.AddColumn<string>(
                name: "PS",
                table: "UserPermissions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PS",
                table: "UserPermissions");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "UserPermissions",
                newName: "UserId");
        }
    }
}
