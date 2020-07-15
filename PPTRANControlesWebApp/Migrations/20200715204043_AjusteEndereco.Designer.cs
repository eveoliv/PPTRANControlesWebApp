﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PPTRANControlesWebApp.Data;

namespace PPTRANControlesWebApp.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20200715204043_AjusteEndereco")]
    partial class AjusteEndereco
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Models.Agenda", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("ClienteId");

                    b.Property<long?>("ClinicaId");

                    b.Property<DateTime>("Data");

                    b.Property<string>("IdUser");

                    b.Property<long?>("MedicoId");

                    b.Property<long?>("PsicologoId");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.HasIndex("ClinicaId");

                    b.ToTable("Agendas");
                });

            modelBuilder.Entity("Models.Caixa", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("ClienteId");

                    b.Property<long?>("ClinicaId");

                    b.Property<DateTime>("Data");

                    b.Property<int>("FormaPgto");

                    b.Property<long?>("HistoricoId");

                    b.Property<string>("IdUser");

                    b.Property<long?>("ProdutoId");

                    b.Property<string>("Ref");

                    b.Property<int>("Tipo");

                    b.Property<decimal>("Valor");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.HasIndex("ClinicaId");

                    b.HasIndex("HistoricoId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("Caixas");
                });

            modelBuilder.Entity("Models.Cliente", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CNH");

                    b.Property<string>("CPF");

                    b.Property<int>("Categoria");

                    b.Property<long?>("ClinicaId");

                    b.Property<DateTime>("DtCadastro");

                    b.Property<DateTime>("DtHabHum");

                    b.Property<DateTime>("DtNascimento");

                    b.Property<string>("Email");

                    b.Property<long?>("EnderecoId");

                    b.Property<string>("Escolaridade");

                    b.Property<string>("EstadoCivil");

                    b.Property<long?>("HistoricoId");

                    b.Property<string>("IdUser");

                    b.Property<string>("Mae");

                    b.Property<long?>("MedicoId");

                    b.Property<string>("Nacionalidade");

                    b.Property<string>("Naturalidade");

                    b.Property<string>("Nome");

                    b.Property<string>("NumLaudo");

                    b.Property<string>("NumRenach");

                    b.Property<string>("Pai");

                    b.Property<string>("Profissao");

                    b.Property<long?>("PsicologoId");

                    b.Property<string>("RG");

                    b.Property<int>("Status");

                    b.Property<int>("StatusPgto");

                    b.Property<string>("Telefone");

                    b.HasKey("Id");

                    b.HasIndex("ClinicaId");

                    b.HasIndex("EnderecoId")
                        .IsUnique();

                    b.HasIndex("HistoricoId");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("Models.Clinica", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alias");

                    b.Property<string>("CNPJ");

                    b.Property<string>("Email");

                    b.Property<long?>("EnderecoId");

                    b.Property<string>("IdUser");

                    b.Property<string>("Nome");

                    b.Property<int>("Status");

                    b.Property<string>("Tel1");

                    b.Property<string>("Tel2");

                    b.HasKey("Id");

                    b.HasIndex("EnderecoId")
                        .IsUnique();

                    b.ToTable("Clinicas");
                });

            modelBuilder.Entity("Models.Colaborador", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CPF");

                    b.Property<string>("CRM");

                    b.Property<string>("CRP");

                    b.Property<long?>("ClinicaId");

                    b.Property<DateTime>("DtCadastro");

                    b.Property<string>("Email");

                    b.Property<long?>("EnderecoId");

                    b.Property<int>("Funcao");

                    b.Property<string>("IdUser");

                    b.Property<string>("Nome");

                    b.Property<string>("RG");

                    b.Property<int>("Status");

                    b.Property<string>("Telefone");

                    b.HasKey("Id");

                    b.HasIndex("ClinicaId");

                    b.HasIndex("EnderecoId")
                        .IsUnique();

                    b.ToTable("Colaboradores");
                });

            modelBuilder.Entity("Models.Contato", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descricao");

                    b.Property<string>("Email");

                    b.Property<long?>("EnderecoId");

                    b.Property<string>("IdUser");

                    b.Property<string>("Nome");

                    b.Property<string>("Tel1");

                    b.Property<string>("Tel2");

                    b.HasKey("Id");

                    b.HasIndex("EnderecoId");

                    b.ToTable("Contatos");
                });

            modelBuilder.Entity("Models.Endereco", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bairro");

                    b.Property<string>("Cep");

                    b.Property<string>("Cidade");

                    b.Property<string>("Complto");

                    b.Property<string>("Estado");

                    b.Property<string>("IdUser");

                    b.Property<long?>("Numero");

                    b.Property<string>("Rua");

                    b.HasKey("Id");

                    b.ToTable("Enderecos");
                });

            modelBuilder.Entity("Models.Historico", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("IdUser");

                    b.Property<string>("Nome");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("Historicos");
                });

            modelBuilder.Entity("Models.Produto", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("IdUser");

                    b.Property<string>("Nome");

                    b.Property<int>("Status");

                    b.Property<decimal>("Valor");

                    b.HasKey("Id");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("Models.Agenda", b =>
                {
                    b.HasOne("Models.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteId");

                    b.HasOne("Models.Clinica", "Clinica")
                        .WithMany()
                        .HasForeignKey("ClinicaId");
                });

            modelBuilder.Entity("Models.Caixa", b =>
                {
                    b.HasOne("Models.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteId");

                    b.HasOne("Models.Clinica", "Clinica")
                        .WithMany()
                        .HasForeignKey("ClinicaId");

                    b.HasOne("Models.Historico", "Historico")
                        .WithMany()
                        .HasForeignKey("HistoricoId");

                    b.HasOne("Models.Produto", "Produto")
                        .WithMany()
                        .HasForeignKey("ProdutoId");
                });

            modelBuilder.Entity("Models.Cliente", b =>
                {
                    b.HasOne("Models.Clinica", "Clinica")
                        .WithMany("Cliente")
                        .HasForeignKey("ClinicaId");

                    b.HasOne("Models.Endereco", "Endereco")
                        .WithOne("Cliente")
                        .HasForeignKey("Models.Cliente", "EnderecoId");

                    b.HasOne("Models.Historico", "Historico")
                        .WithMany("Cliente")
                        .HasForeignKey("HistoricoId");
                });

            modelBuilder.Entity("Models.Clinica", b =>
                {
                    b.HasOne("Models.Endereco", "Endereco")
                        .WithOne("Clinica")
                        .HasForeignKey("Models.Clinica", "EnderecoId");
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

            modelBuilder.Entity("Models.Contato", b =>
                {
                    b.HasOne("Models.Endereco", "Endereco")
                        .WithMany()
                        .HasForeignKey("EnderecoId");
                });
#pragma warning restore 612, 618
        }
    }
}
