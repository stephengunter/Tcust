using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Blog.Migrations
{
    public partial class addPostDepartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostsDepartments",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false),
                    DepartmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostsDepartments", x => new { x.PostId, x.DepartmentId });
                });

            migrationBuilder.CreateTable(
                name: "PostsIssuers",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false),
                    DepartmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostsIssuers", x => new { x.PostId, x.DepartmentId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostsDepartments");

            migrationBuilder.DropTable(
                name: "PostsIssuers");
        }
    }
}
