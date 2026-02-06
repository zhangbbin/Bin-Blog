using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bin_Blog.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "tb_blog_post",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tb_category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_category", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tb_blog_post_CategoryId",
                table: "tb_blog_post",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_blog_post_tb_category_CategoryId",
                table: "tb_blog_post",
                column: "CategoryId",
                principalTable: "tb_category",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_blog_post_tb_category_CategoryId",
                table: "tb_blog_post");

            migrationBuilder.DropTable(
                name: "tb_category");

            migrationBuilder.DropIndex(
                name: "IX_tb_blog_post_CategoryId",
                table: "tb_blog_post");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "tb_blog_post");
        }
    }
}
