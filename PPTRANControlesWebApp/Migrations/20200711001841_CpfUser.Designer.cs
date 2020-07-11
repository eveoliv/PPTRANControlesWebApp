﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PPTRANControlesWebApp.Data;

namespace PPTRANControlesWebApp.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20200711001841_CpfUser")]
    partial class CpfUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Models.Agenda", b =>
                {
                    b.Property<long?>("AgendaId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("ClienteId");

                    b.Property<string>("CpfUser");

                    b.Property<DateTime>("Data");

                    b.Property<long?>("MedicoId");

                    b.Property<long?>("PsicologoId");

                    b.HasKey("AgendaId");

                    b.ToTable("Agendas");
                });

            modelBuilder.Entity("Models.Caixa", b =>
                {
                    b.Property<long?>("CaixaId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("ClienteId");

                    b.Property<long?>("ClinicaId");

                    b.Property<long?>("ColaboradorId");

                    b.Property<string>("CpfUser");

                    b.Property<DateTime>("Data");

                    b.Property<int>("FormaPgto");

                    b.Property<string>("Historico");

                    b.Property<string>("Ref");

                    b.Property<int>("Tipo");

                    b.Property<decimal>("Valor");

                    b.HasKey("CaixaId");

                    b.HasIndex("ClienteId");

                    b.HasIndex("ClinicaId");

                    b.HasIndex("ColaboradorId");

                    b.ToTable("Caixas");
                });

            modelBuilder.Entity("Models.Cliente", b =>
                {
                    b.Property<long?>("ClienteId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CNH");

                    b.Property<string>("CPF");

                    b.Property<int>("Categoria");

                    b.Property<long?>("ClinicaId");

                    b.Property<string>("CpfUser");

                    b.Property<DateTime>("DtCadastro");

                    b.Property<DateTime>("DtHabHum");

                    b.Property<DateTime>("DtNascimento");

                    b.Property<string>("Email");

                    b.Property<long?>("EnderecoId");

                    b.Property<long?>("EntrevistaId");

                    b.Property<string>("Escolaridade");

                    b.Property<string>("EstadoCivil");

                    b.Property<string>("Mae");

                    b.Property<string>("Nacionalidade");

                    b.Property<string>("Naturalidade");

                    b.Property<string>("Nome");

                    b.Property<string>("NumLaudo");

                    b.Property<string>("NumRenach");

                    b.Property<string>("Pai");

                    b.Property<string>("PerfilPsicologico");

                    b.Property<string>("Profissao");

                    b.Property<string>("RG");

                    b.Property<int>("Status");

                    b.Property<string>("Telefone");

                    b.HasKey("ClienteId");

                    b.HasIndex("ClinicaId");

                    b.HasIndex("EnderecoId")
                        .IsUnique();

                    b.HasIndex("EntrevistaId")
                        .IsUnique();

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("Models.Clinica", b =>
                {
                    b.Property<long?>("ClinicaId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alias");

                    b.Property<string>("CNPJ");

                    b.Property<string>("CpfUser");

                    b.Property<string>("Email");

                    b.Property<long?>("EnderecoId");

                    b.Property<string>("Nome");

                    b.Property<int>("Status");

                    b.Property<string>("Tel1");

                    b.Property<string>("Tel2");

                    b.HasKey("ClinicaId");

                    b.HasIndex("EnderecoId");

                    b.ToTable("Clinicas");
                });

            modelBuilder.Entity("Models.Colaborador", b =>
                {
                    b.Property<long?>("ColaboradorId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CPF");

                    b.Property<string>("CRM");

                    b.Property<string>("CRP");

                    b.Property<long?>("ClinicaId");

                    b.Property<string>("CpfUser");

                    b.Property<DateTime>("DtCadastro");

                    b.Property<string>("Email");

                    b.Property<long?>("EnderecoId");

                    b.Property<string>("Funcao");

                    b.Property<string>("Nome");

                    b.Property<string>("RG");

                    b.Property<int>("Status");

                    b.Property<string>("Telefone");

                    b.HasKey("ColaboradorId");

                    b.HasIndex("ClinicaId");

                    b.HasIndex("EnderecoId")
                        .IsUnique();

                    b.ToTable("Colaboradores");
                });

            modelBuilder.Entity("Models.Endereco", b =>
                {
                    b.Property<long?>("EnderecoId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bairro");

                    b.Property<string>("CPF");

                    b.Property<string>("Cep");

                    b.Property<string>("Cidade");

                    b.Property<string>("Complto");

                    b.Property<string>("Estado");

                    b.Property<long?>("Numero");

                    b.Property<string>("Rua");

                    b.HasKey("EnderecoId");

                    b.ToTable("Enderecos");
                });

            modelBuilder.Entity("Models.Entrevista", b =>
                {
                    b.Property<long?>("EntrevistaId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Acidente");

                    b.Property<int>("Alimentacao");

                    b.Property<int>("AtividadeRem");

                    b.Property<int>("Bebida");

                    b.Property<string>("CPF");

                    b.Property<int>("Cafe");

                    b.Property<DateTime>("DataForm");

                    b.Property<string>("Declaracao");

                    b.Property<int>("Desmaios");

                    b.Property<int>("Doencas");

                    b.Property<string>("DoencasObs");

                    b.Property<int>("Dominante");

                    b.Property<int>("DorCabeca");

                    b.Property<int>("Drogas");

                    b.Property<string>("DrogasObs");

                    b.Property<int>("Habilitado");

                    b.Property<int>("Insonia");

                    b.Property<int>("Medicamento");

                    b.Property<string>("MedicamentoObs");

                    b.Property<string>("Parecer");

                    b.Property<int>("Pcd");

                    b.Property<string>("PcdObs");

                    b.Property<string>("ProcMedicoObs");

                    b.Property<int>("ProcMedido");

                    b.Property<long?>("PsicologoId");

                    b.Property<int>("RelFamiliar");

                    b.Property<int>("Tabagismo");

                    b.Property<string>("TabagismoObs");

                    b.Property<int>("Tonturas");

                    b.Property<int>("TratPsicologico");

                    b.HasKey("EntrevistaId");

                    b.ToTable("Entrevista");
                });

            modelBuilder.Entity("Models.Caixa", b =>
                {
                    b.HasOne("Models.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteId");

                    b.HasOne("Models.Clinica", "Clinica")
                        .WithMany()
                        .HasForeignKey("ClinicaId");

                    b.HasOne("Models.Colaborador", "Colaborador")
                        .WithMany()
                        .HasForeignKey("ColaboradorId");
                });

            modelBuilder.Entity("Models.Cliente", b =>
                {
                    b.HasOne("Models.Clinica", "Clinica")
                        .WithMany("Cliente")
                        .HasForeignKey("ClinicaId");

                    b.HasOne("Models.Endereco", "Endereco")
                        .WithOne("Cliente")
                        .HasForeignKey("Models.Cliente", "EnderecoId");

                    b.HasOne("Models.Entrevista", "Entrevista")
                        .WithOne("Cliente")
                        .HasForeignKey("Models.Cliente", "EntrevistaId");
                });

            modelBuilder.Entity("Models.Clinica", b =>
                {
                    b.HasOne("Models.Endereco", "Endereco")
                        .WithMany()
                        .HasForeignKey("EnderecoId");
                });

            modelBuilder.Entity("Models.Colaborador", b =>
                {
                    b.HasOne("Models.Clinica", "Clinica")
                        .WithMany("Colaborador")
                        .HasForeignKey("ClinicaId");

                    b.HasOne("Models.Endereco", "Endereco")
                        .WithOne("Colaborador")
                        .HasForeignKey("Models.Colaborador", "EnderecoId");
                });
#pragma warning restore 612, 618
        }
    }
}
