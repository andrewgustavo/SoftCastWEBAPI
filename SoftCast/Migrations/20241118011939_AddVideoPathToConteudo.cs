using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftCast.Migrations
{
    /// <inheritdoc />
    public partial class AddVideoPathToConteudo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoPath",
                table: "Conteudos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoPath",
                table: "Conteudos");
        }
    }
}
