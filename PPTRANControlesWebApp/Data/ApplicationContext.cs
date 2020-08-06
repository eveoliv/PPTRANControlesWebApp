using Models;
using Microsoft.EntityFrameworkCore;

namespace PPTRANControlesWebApp.Data
{
    public class ApplicationContext : DbContext
    {
        //Construtora para as configurações do contexto
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        //Mapeamento do modelo relacional
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Agenda> Agendas { get; set; }
        public DbSet<Caixa> Caixas { get; set; }
        public DbSet<Clinica> Clinicas { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }     
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Contato> Contatos { get; set; }
        public DbSet<Historico> Historicos { get; set; }
        public DbSet<Carrinho> Carrinhos { get; set; }

        //Sobrescrita do nome das tabelas, caso seja necessario
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Colaborador>().HasKey(c => c.Id);
            modelBuilder.Entity<Colaborador>().HasOne(c => c.Clinica);
            modelBuilder.Entity<Colaborador>().HasOne(c => c.Endereco);

            modelBuilder.Entity<Cliente>().HasKey(c => c.Id);
            modelBuilder.Entity<Cliente>().HasOne(c => c.Clinica);
            modelBuilder.Entity<Cliente>().HasOne(c => c.Endereco);
            modelBuilder.Entity<Cliente>().HasOne(c => c.Historico);

            modelBuilder.Entity<Endereco>().HasKey(c => c.Id);
        }

        //Sobrescrita do metodo de acesso ao db
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder
            //    .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=IESUtfpr;Trusted_Connection=True;MultipleActiveResultSets= true");
        }
    }
}
