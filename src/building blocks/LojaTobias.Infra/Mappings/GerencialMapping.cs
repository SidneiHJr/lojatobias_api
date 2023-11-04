using LojaTobias.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LojaTobias.Infra.Mappings
{
    public class LogMapping : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("Log");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("LogId");

            builder.Property(p => p.UsuarioCriacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.UsuarioAtualizacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.Usuario)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.Acao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.Tipo)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.Mensagem)
                .HasColumnType("varchar")
                .HasMaxLength(1000);
        }
    }
}
