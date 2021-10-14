using Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PPTRANControlesWebApp.Data
{
    internal class RepasseConfiguration : IEntityTypeConfiguration<Repasse>
    {
        public void Configure(EntityTypeBuilder<Repasse> builder)
        {
            builder.ToTable("Repasses");
            builder.Property(i => i.Id).HasColumnName("Id");
            builder.Property(p => p.Profissional).HasColumnName("Profissional");
            builder.Property(v => v.Valor).HasColumnName("Valor");
            builder.Property(i => i.IdUser).HasColumnName("idUser");
            builder.Property(c => c.ClinicaId).HasColumnName("ClinicaId");

            //exemplo
            //builder.ToTable("actor");
            //builder.Property(i => i.Id).HasColumnName("actor_id");
            //builder.Property(p => p.PrimeiroNome).HasColumnName("first_name").HasColumnType("varchar(45)").IsRequired();
            //builder.Property(s => s.UltimoNome).HasColumnName("last_name").HasColumnType("varchar(45)").IsRequired();
            //builder.Property<DateTime>("last_update").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            //builder.HasIndex(a => a.UltimoNome);
        }
    }
}