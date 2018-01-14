using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Blog.Migrations
{
    public partial class editPermission8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissons_AppUsers_AppUserId",
                table: "Permissons");

            migrationBuilder.DropIndex(
                name: "IX_Permissons_AppUserId",
                table: "Permissons");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Permissons");

            migrationBuilder.CreateTable(
                name: "UserPermission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    PermissionId = table.Column<int>(nullable: false),
                    Removed = table.Column<bool>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPermission_Permissons_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermission_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_PermissionId",
                table: "UserPermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_UserId",
                table: "UserPermission",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPermission");

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Permissons",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissons_AppUserId",
                table: "Permissons",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissons_AppUsers_AppUserId",
                table: "Permissons",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
