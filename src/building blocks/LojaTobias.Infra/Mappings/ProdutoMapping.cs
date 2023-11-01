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

            builder.HasMany(p => p.ProdutosUnidade)
                .WithOne(x => x.Produto)
                .HasForeignKey(x => x.Id);

        }
    }

    public class ProdutoUnidadeMapping : IEntityTypeConfiguration<ProdutoUnidade>
    {
        public void Configure(EntityTypeBuilder<ProdutoUnidade> builder)
        {
            builder.ToTable("ProdutoUnidade");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("ProdutoUnidadeId");

            builder.HasOne(p => p.UnidadeMedida)
                .WithMany(x => x.ProdutosUnidade)
                .HasForeignKey(p => p.Id);

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
