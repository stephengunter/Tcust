using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Blog.Migrations
{
    public partial class userPermissionupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPermissions",
                table: "UserPermissions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserPermissions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserPermissions");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "UserPermissions");

            migrationBuilder.DropColumn(
                name: "PS",
                table: "UserPermissions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "UserPermissions");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "UserPermissions");

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "UserPermissions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPermissions",
                table: "UserPermissions",
                columns: new[] { "AppUserId", "PermissionId" });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PS = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissions_AppUsers_AppUserId",
                table: "UserPermissions",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissions_AppUsers_AppUserId",
                table: "UserPermissions");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPermissions",
                table: "UserPermissions");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "UserPermissions");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserPermissions",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserPermissions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "UserPermissions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PS",
                table: "UserPermissions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "UserPermissions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "UserPermissions",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPermissions",
                table: "UserPermissions",
                column: "Id");
        }
    }
}
