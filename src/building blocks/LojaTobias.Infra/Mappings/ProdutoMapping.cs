using LojaTobias.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaTobias.Infra.Mappings
{
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produto");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("ProdutoId");

            builder.HasOne(p => p.UnidadeMedida)
                .WithMany(x => x.Produtos)
                .HasForeignKey(p => p.UnidadeMedidaId);

        }
    }

    public class UnidadeMedidaMapping : IEntityTypeConfiguration<UnidadeMedida>
    {
        public void Configure(EntityTypeBuilder<UnidadeMedida> builder)
        {
            builder.ToTable("UnidadeMedida");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("UnidadeMedidaId");

        }
    }
}
