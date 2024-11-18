using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftCast.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailAndSenhaToCriador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Criadores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Senha",
                table: "Criadores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Criadores");

            migrationBuilder.DropColumn(
                name: "Senha",
                table: "Criadores");
        }
    }
}
