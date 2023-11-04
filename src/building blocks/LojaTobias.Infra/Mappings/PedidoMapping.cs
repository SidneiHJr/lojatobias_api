using LojaTobias.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LojaTobias.Infra.Mappings
{
    public class PedidoMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedido");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("PedidoId");

            builder.Property(p => p.UsuarioCriacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.UsuarioAtualizacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.Tipo)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.Observacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.Fornecedor)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.Cliente)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.Status)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("PedidoId");

        }
    }

    public class PedidoItemMapping : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            builder.ToTable("PedidoItem");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("PedidoItemId");

            builder.Property(p => p.UsuarioCriacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.UsuarioAtualizacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.Observacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.HasOne(p => p.Pedido)
                .WithMany(x => x.Produtos)
                .HasForeignKey(p => p.PedidoId);

            builder.HasOne(p => p.Produto)
                .WithMany(x => x.Pedidos)
                .HasForeignKey(p => p.ProdutoId);

            builder.HasOne(p => p.UnidadeMedida)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
