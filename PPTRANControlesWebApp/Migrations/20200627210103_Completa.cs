using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PPTRANControlesWebApp.Migrations
{
    public partial class Completa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agendas",
                columns: table => new
                {
                    AgendaId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Data = table.Column<DateTime>(nullable: false),
                    ClienteId = table.Column<long>(nullable: true),
                    PsicologoId = table.Column<long>(nullable: true),
                    MedicoId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agendas", x => x.AgendaId);
                });

            migrationBuilder.CreateTable(
                name: "Caixas",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Tipo = table.Column<int>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    Saldo = table.Column<decimal>(nullable: false),
                    Lancamento = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caixas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clinicas",
                columns: table => new
                {
                    ClinicaId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    Endereco = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinicas", x => x.ClinicaId);
                });

            migrationBuilder.CreateTable(
                name: "Enderecos",
                columns: table => new
                {
                    EnderecoId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CPF = table.Column<string>(nullable: true),
                    Cep = table.Column<string>(nullable: true),
                    Rua = table.Column<string>(nullable: true),
                    Bairro = table.Column<string>(nullable: true),
                    Cidade = table.Column<string>(nullable: true),
                    Estado = table.Column<string>(nullable: true),
                    Numero = table.Column<long>(nullable: true),
                    Complto = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enderecos", x => x.EnderecoId);
                });

            migrationBuilder.CreateTable(
                name: "Entrevistas",
                columns: table => new
                {
                    EntrevistaId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CPF = table.Column<string>(nullable: true),
                    Tabagismo = table.Column<int>(nullable: false),
                    TabagismoObs = table.Column<string>(nullable: true),
                    Cafe = table.Column<int>(nullable: false),
                    Bebida = table.Column<int>(nullable: false),
                    Drogas = table.Column<int>(nullable: false),
                    DrogasObs = table.Column<string>(nullable: true),
                    Medicamento = table.Column<int>(nullable: false),
                    MedicamentoObs = table.Column<string>(nullable: true),
                    ProcMedido = table.Column<int>(nullable: false),
                    TratPsicologico = table.Column<int>(nullable: false),
                    Acidente = table.Column<int>(nullable: false),
                    DorCabeca = table.Column<int>(nullable: false),
                    Tonturas = table.Column<int>(nullable: false),
                    Insonia = table.Column<int>(nullable: false),
                    Desmaios = table.Column<int>(nullable: false),
                    Doencas = table.Column<int>(nullable: false),
                    DoencasObs = table.Column<string>(nullable: true),
                    Alimentacao = table.Column<int>(nullable: false),
                    Dominante = table.Column<int>(nullable: false),
                    RelFamiliar = table.Column<int>(nullable: false),
                    Pcd = table.Column<int>(nullable: false),
                    PcdObs = table.Column<string>(nullable: true),
                    Habilitado = table.Column<int>(nullable: false),
                    AtividadeRem = table.Column<int>(nullable: false),
                    Declaracao = table.Column<string>(nullable: true),
                    Parecer = table.Column<string>(nullable: true),
                    DataForm = table.Column<DateTime>(nullable: false),
                    PsicologoId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrevistas", x => x.EntrevistaId);
                });

            migrationBuilder.CreateTable(
                name: "Colaboradores",
                columns: table => new
                {
                    ColaboradorId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    RG = table.Column<string>(nullable: true),
                    CPF = table.Column<string>(nullable: true),
                    CRP = table.Column<string>(nullable: true),
                    CRM = table.Column<string>(nullable: true),
                    Endereco = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(nullable: true),
                    Funcao = table.Column<string>(nullable: true),
                    ClinicaId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colaboradores", x => x.ColaboradorId);
                    table.ForeignKey(
                        name: "FK_Colaboradores_Clinicas_ClinicaId",
                        column: x => x.ClinicaId,
                        principalTable: "Clinicas",
                        principalColumn: "ClinicaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    ClienteId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    CPF = table.Column<string>(nullable: true),
                    RG = table.Column<string>(nullable: true),
                    CNH = table.Column<string>(nullable: true),
                    Categoria = table.Column<int>(nullable: false),
                    NumRenach = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(nullable: true),
                    PerfilPsicologico = table.Column<string>(nullable: true),
                    NumLaudo = table.Column<string>(nullable: true),
                    DtHabHum = table.Column<DateTime>(nullable: false),
                    Pai = table.Column<string>(nullable: true),
                    Mae = table.Column<string>(nullable: true),
                    Nacionalidade = table.Column<string>(nullable: true),
                    Naturalidade = table.Column<string>(nullable: true),
                    EstadoCivil = table.Column<string>(nullable: true),
                    DtNascimento = table.Column<DateTime>(nullable: false),
                    Profissao = table.Column<string>(nullable: true),
                    Escolaridade = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ClinicaId = table.Column<long>(nullable: true),
                    EnderecoId = table.Column<long>(nullable: true),
                    EntrevistaId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.ClienteId);
                    table.ForeignKey(
                        name: "FK_Clientes_Clinicas_ClinicaId",
                        column: x => x.ClinicaId,
                        principalTable: "Clinicas",
                        principalColumn: "ClinicaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clientes_Enderecos_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Enderecos",
                        principalColumn: "EnderecoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clientes_Entrevistas_EntrevistaId",
                        column: x => x.EntrevistaId,
                        principalTable: "Entrevistas",
                        principalColumn: "EntrevistaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_ClinicaId",
                table: "Clientes",
                column: "ClinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_EnderecoId",
                table: "Clientes",
                column: "EnderecoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_EntrevistaId",
                table: "Clientes",
                column: "EntrevistaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_ClinicaId",
                table: "Colaboradores",
                column: "ClinicaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agendas");

            migrationBuilder.DropTable(
                name: "Caixas");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Colaboradores");

            migrationBuilder.DropTable(
                name: "Enderecos");

            migrationBuilder.DropTable(
                name: "Entrevistas");

            migrationBuilder.DropTable(
                name: "Clinicas");
        }
    }
}
