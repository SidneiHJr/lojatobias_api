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

            builder.Property(p => p.UsuarioCriacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.UsuarioAtualizacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.Nome)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.Descricao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.HasOne(p => p.UnidadeMedida);

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

            builder.Property(p => p.UsuarioCriacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.UsuarioAtualizacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.Nome)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.Abreviacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

        }
    }

    public class UnidadeMedidaConversaoMapping : IEntityTypeConfiguration<UnidadeMedidaConversao>
    {
        public void Configure(EntityTypeBuilder<UnidadeMedidaConversao> builder)
        {
            builder.ToTable("UnidadeMedidaConversao");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("UnidadeMedidaConversaoId");

            builder.Property(p => p.UsuarioCriacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.UsuarioAtualizacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.HasOne(p => p.UnidadeMedidaEntrada)
                .WithMany(x => x.ConversoesEntrada)
                .HasForeignKey(p => p.UnidadeMedidaEntradaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.UnidadeMedidaSaida)
                .WithMany(x => x.ConversoesSaida)
                .HasForeignKey(p => p.UnidadeMedidaSaidaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
