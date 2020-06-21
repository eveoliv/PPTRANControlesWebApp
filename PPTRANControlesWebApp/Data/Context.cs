using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data
{
    public class Context : DbContext
    {
        //Construtora para as configurações do contexto
        public Context(DbContextOptions<Context> options) : base(options) { }

        //Mapeamento do modelo relacional
        //public DbSet<Agenda> Agenda { get; set; }
        //public DbSet<Caixa> Caixa { get; set; }
        //public DbSet<Colaborador> Colaborador { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }

        //Sobrescrita do nome das tabelas, caso seja necessario
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Foo>().ToTable("Bar");
        }

        //Sobrescrita do metodo de acesso ao db
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder
            //    .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=IESUtfpr;Trusted_Connection=True;MultipleActiveResultSets= true");
        }
    }
}
