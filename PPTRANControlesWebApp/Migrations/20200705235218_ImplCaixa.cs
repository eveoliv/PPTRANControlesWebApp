using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PPTRANControlesWebApp.Migrations
{
    public partial class ImplCaixa : Migration
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
                name: "Entrevista",
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
                    ProcMedicoObs = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_Entrevista", x => x.EntrevistaId);
                });

            migrationBuilder.CreateTable(
                name: "Clinicas",
                columns: table => new
                {
                    ClinicaId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    CNPJ = table.Column<string>(nullable: true),
                    Alias = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Tel1 = table.Column<string>(nullable: true),
                    Tel2 = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    EnderecoId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinicas", x => x.ClinicaId);
                    table.ForeignKey(
                        name: "FK_Clinicas_Enderecos_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Enderecos",
                        principalColumn: "EnderecoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    ClienteId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DtCadastro = table.Column<DateTime>(nullable: false),
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
                        name: "FK_Clientes_Entrevista_EntrevistaId",
                        column: x => x.EntrevistaId,
                        principalTable: "Entrevista",
                        principalColumn: "EntrevistaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Colaboradores",
                columns: table => new
                {
                    ColaboradorId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DtCadastro = table.Column<DateTime>(nullable: false),
                    Nome = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    RG = table.Column<string>(nullable: true),
                    CPF = table.Column<string>(nullable: true),
                    CRP = table.Column<string>(nullable: true),
                    CRM = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(nullable: true),
                    Funcao = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ClinicaId = table.Column<long>(nullable: true),
                    EnderecoId = table.Column<long>(nullable: true)
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
                    table.ForeignKey(
                        name: "FK_Colaboradores_Enderecos_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Enderecos",
                        principalColumn: "EnderecoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Caixas",
                columns: table => new
                {
                    CaixaId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Data = table.Column<DateTime>(nullable: false),
                    Historico = table.Column<string>(nullable: true),
                    Tipo = table.Column<int>(nullable: false),
                    Valor = table.Column<decimal>(nullable: false),
                    FormaPgto = table.Column<int>(nullable: false),
                    Ref = table.Column<string>(nullable: true),
                    ClienteId = table.Column<long>(nullable: true),
                    ClinicaId = table.Column<long>(nullable: true),
                    ColaboradorId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caixas", x => x.CaixaId);
                    table.ForeignKey(
                        name: "FK_Caixas_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "ClienteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Caixas_Clinicas_ClinicaId",
                        column: x => x.ClinicaId,
                        principalTable: "Clinicas",
                        principalColumn: "ClinicaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Caixas_Colaboradores_ColaboradorId",
                        column: x => x.ColaboradorId,
                        principalTable: "Colaboradores",
                        principalColumn: "ColaboradorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Caixas_ClienteId",
                table: "Caixas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Caixas_ClinicaId",
                table: "Caixas",
                column: "ClinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Caixas_ColaboradorId",
                table: "Caixas",
                column: "ColaboradorId");

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
                name: "IX_Clinicas_EnderecoId",
                table: "Clinicas",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_ClinicaId",
                table: "Colaboradores",
                column: "ClinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_EnderecoId",
                table: "Colaboradores",
                column: "EnderecoId",
                unique: true);
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
                name: "Entrevista");

            migrationBuilder.DropTable(
                name: "Clinicas");

            migrationBuilder.DropTable(
                name: "Enderecos");
        }
    }
}
