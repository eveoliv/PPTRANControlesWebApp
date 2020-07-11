using Microsoft.EntityFrameworkCore.Migrations;

namespace PPTRANControlesWebApp.Migrations
{
    public partial class CpfUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CpfUser",
                table: "Colaboradores",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CpfUser",
                table: "Clinicas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CpfUser",
                table: "Clientes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CpfUser",
                table: "Caixas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CpfUser",
                table: "Agendas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CpfUser",
                table: "Colaboradores");

            migrationBuilder.DropColumn(
                name: "CpfUser",
                table: "Clinicas");

            migrationBuilder.DropColumn(
                name: "CpfUser",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "CpfUser",
                table: "Caixas");

            migrationBuilder.DropColumn(
                name: "CpfUser",
                table: "Agendas");
        }
    }
}
