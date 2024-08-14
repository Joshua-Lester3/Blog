using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Blogs",
                table: "Blogs");

            migrationBuilder.RenameTable(
                name: "Blogs",
                newName: "BlogPosts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogPosts",
                table: "BlogPosts",
                column: "BlogPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogPosts",
                table: "BlogPosts");

            migrationBuilder.RenameTable(
                name: "BlogPosts",
                newName: "Blogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Blogs",
                table: "Blogs",
                column: "BlogPostId");
        }
    }
}
