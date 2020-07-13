using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data
{
    public class ApplicationContext : DbContext
    {
        //Construtora para as configurações do contexto
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        //Mapeamento do modelo relacional
        public DbSet<Agenda> Agendas { get; set; }
        public DbSet<Caixa> Caixas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Clinica> Clinicas { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }       

        //Sobrescrita do nome das tabelas, caso seja necessario
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Colaborador>().HasKey(c => c.Id);
            modelBuilder.Entity<Colaborador>().HasOne(c => c.Clinica);
            modelBuilder.Entity<Colaborador>().HasOne(c => c.Endereco);

            //modelBuilder.Entity<Agenda>().HasKey(c => c.Id);
            //modelBuilder.Entity<Caixa>().HasKey(c => c.Id);
            //modelBuilder.Entity<Cliente>().HasKey(c => c.Id);
            //modelBuilder.Entity<Clinica>().HasKey(c => c.Id);
            //modelBuilder.Entity<Endereco>().HasKey(c => c.Id);
            
            
            
        }

        //Sobrescrita do metodo de acesso ao db
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder
            //    .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=IESUtfpr;Trusted_Connection=True;MultipleActiveResultSets= true");
        }
    }
}
