using LojaTobias.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaTobias.Infra.Mappings
{
    public class AjusteMapping : IEntityTypeConfiguration<Ajuste>
    {
        public void Configure(EntityTypeBuilder<Ajuste> builder)
        {
            builder.ToTable("Ajuste");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("AjusteId");

            builder.Property(p => p.UsuarioCriacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.UsuarioAtualizacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.Tipo)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.Motivo)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.HasOne(p => p.UnidadeMedida);

            builder.HasOne(p => p.Produto)
                .WithMany(x => x.Ajustes)
                .HasForeignKey(p => p.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
