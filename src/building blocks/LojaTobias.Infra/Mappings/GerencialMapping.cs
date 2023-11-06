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

    public class MovimentacaoMapping : IEntityTypeConfiguration<Movimentacao>
    {
        public void Configure(EntityTypeBuilder<Movimentacao> builder)
        {
            builder.ToTable("Movimentacao");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("MovimentacaoId");

            builder.Property(p => p.UsuarioCriacao)
               .HasColumnType("varchar")
               .HasMaxLength(1000);

            builder.Property(p => p.UsuarioAtualizacao)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(p => p.Categoria)
               .HasColumnType("varchar")
               .HasMaxLength(1000);

            builder.Property(p => p.Tipo)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.HasOne(p => p.Ajuste);

            builder.HasOne(p => p.Caixa)
                .WithMany(x => x.Movimentacoes)
                .HasForeignKey(p => p.CaixaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Produto)
                .WithMany(x => x.Movimentacoes)
                .HasForeignKey(p => p.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Pedido)
                .WithMany(x => x.Movimentacoes)
                .HasForeignKey(p => p.PedidoId)
                .OnDelete(DeleteBehavior.Restrict);



        }
    }
}
