using Microsoft.EntityFrameworkCore.Migrations;

namespace PPTRANControlesWebApp.Migrations
{
    public partial class MedicEPsi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Colaboradores_ColaboradorId",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_ColaboradorId",
                table: "Clientes");

            migrationBuilder.RenameColumn(
                name: "ColaboradorId",
                table: "Clientes",
                newName: "PsicologoId");

            migrationBuilder.AddColumn<long>(
                name: "MedicoId",
                table: "Clientes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MedicoId",
                table: "Clientes");

            migrationBuilder.RenameColumn(
                name: "PsicologoId",
                table: "Clientes",
                newName: "ColaboradorId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_ColaboradorId",
                table: "Clientes",
                column: "ColaboradorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Colaboradores_ColaboradorId",
                table: "Clientes",
                column: "ColaboradorId",
                principalTable: "Colaboradores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
