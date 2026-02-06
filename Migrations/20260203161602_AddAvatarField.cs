using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bin_Blog.Migrations
{
    /// <inheritdoc />
    public partial class AddAvatarField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "tb_user",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "tb_user",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "tb_user");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "tb_user");
        }
    }
}
