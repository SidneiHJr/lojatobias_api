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

            builder.OwnsOne(p => p.Email, email =>
            {
                email.Property(x => x.Endereco).HasColumnName("Email");
            });
        }
    }
}
