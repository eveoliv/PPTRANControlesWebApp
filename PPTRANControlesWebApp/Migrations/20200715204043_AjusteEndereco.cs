using Microsoft.EntityFrameworkCore.Migrations;

namespace PPTRANControlesWebApp.Migrations
{
    public partial class AjusteEndereco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CPF",
                table: "Enderecos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CPF",
                table: "Enderecos",
                nullable: true);
        }
    }
}
