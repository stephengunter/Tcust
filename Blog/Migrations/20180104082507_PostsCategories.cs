using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Blog.Migrations
{
    public partial class PostsCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCategory_Categories_CategoryId",
                table: "PostCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_PostCategory_Posts_PostId",
                table: "PostCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostCategory",
                table: "PostCategory");

            migrationBuilder.RenameTable(
                name: "PostCategory",
                newName: "PostsCategories");

            migrationBuilder.RenameIndex(
                name: "IX_PostCategory_CategoryId",
                table: "PostsCategories",
                newName: "IX_PostsCategories_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostsCategories",
                table: "PostsCategories",
                columns: new[] { "PostId", "CategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PostsCategories_Categories_CategoryId",
                table: "PostsCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostsCategories_Posts_PostId",
                table: "PostsCategories",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostsCategories_Categories_CategoryId",
                table: "PostsCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_PostsCategories_Posts_PostId",
                table: "PostsCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostsCategories",
                table: "PostsCategories");

            migrationBuilder.RenameTable(
                name: "PostsCategories",
                newName: "PostCategory");

            migrationBuilder.RenameIndex(
                name: "IX_PostsCategories_CategoryId",
                table: "PostCategory",
                newName: "IX_PostCategory_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostCategory",
                table: "PostCategory",
                columns: new[] { "PostId", "CategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategory_Categories_CategoryId",
                table: "PostCategory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategory_Posts_PostId",
                table: "PostCategory",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
