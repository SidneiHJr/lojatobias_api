using LojaTobias.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaTobias.Infra.Mappings
{
    public class CaixaMapping : IEntityTypeConfiguration<Caixa>
    {
        public void Configure(EntityTypeBuilder<Caixa> builder)
        {
            builder.ToTable("Caixa");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("CaixaId");

            builder.Property(p => p.UsuarioCriacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.UsuarioAtualizacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);
        }
    }
}
