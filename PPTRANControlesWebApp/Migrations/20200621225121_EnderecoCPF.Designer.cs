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
    [Migration("20200621225121_EnderecoCPF")]
    partial class EnderecoCPF
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Models.Cliente", b =>
                {
                    b.Property<long?>("ClienteId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CNH");

                    b.Property<string>("CPF");

                    b.Property<int>("Categoria");

                    b.Property<DateTime>("DtHabHum");

                    b.Property<DateTime>("DtNascimento");

                    b.Property<string>("Email");

                    b.Property<long?>("EnderecoId");

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

                    b.HasIndex("EnderecoId");

                    b.ToTable("Clientes");
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

            modelBuilder.Entity("Models.Cliente", b =>
                {
                    b.HasOne("Models.Endereco", "Endereco")
                        .WithMany("Clientes")
                        .HasForeignKey("EnderecoId");
                });
#pragma warning restore 612, 618
        }
    }
}
