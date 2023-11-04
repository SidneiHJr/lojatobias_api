using LojaTobias.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaTobias.Infra.Mappings
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("UsuarioId");

            builder.Property(p => p.UsuarioCriacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.UsuarioAtualizacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.Nome)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.Perfil)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.OwnsOne(p => p.Email, email =>
            {
                email.Property(x => x.Endereco).HasColumnName("Email");
            });
        }
    }
}
