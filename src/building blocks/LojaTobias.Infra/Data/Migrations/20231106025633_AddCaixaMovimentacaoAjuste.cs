using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LojaTobias.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCaixaMovimentacaoAjuste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Caixa",
                columns: table => new
                {
                    CaixaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCriacao = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioAtualizacao = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caixa", x => x.CaixaId);
                });

            migrationBuilder.CreateTable(
                name: "Movimentacao",
                columns: table => new
                {
                    MovimentacaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Categoria = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    Tipo = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Quantidade = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CaixaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PedidoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AjusteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCriacao = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioAtualizacao = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimentacao", x => x.MovimentacaoId);
                    table.ForeignKey(
                        name: "FK_Movimentacao_Caixa_CaixaId",
                        column: x => x.CaixaId,
                        principalTable: "Caixa",
                        principalColumn: "CaixaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movimentacao_Pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedido",
                        principalColumn: "PedidoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movimentacao_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "ProdutoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ajuste",
                columns: table => new
                {
                    AjusteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tipo = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    Motivo = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnidadeMedidaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovimentacaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCriacao = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioAtualizacao = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ajuste", x => x.AjusteId);
                    table.ForeignKey(
                        name: "FK_Ajuste_Movimentacao_MovimentacaoId",
                        column: x => x.MovimentacaoId,
                        principalTable: "Movimentacao",
                        principalColumn: "MovimentacaoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ajuste_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "ProdutoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ajuste_UnidadeMedida_UnidadeMedidaId",
                        column: x => x.UnidadeMedidaId,
                        principalTable: "UnidadeMedida",
                        principalColumn: "UnidadeMedidaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ajuste_MovimentacaoId",
                table: "Ajuste",
                column: "MovimentacaoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ajuste_ProdutoId",
                table: "Ajuste",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ajuste_UnidadeMedidaId",
                table: "Ajuste",
                column: "UnidadeMedidaId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacao_CaixaId",
                table: "Movimentacao",
                column: "CaixaId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacao_PedidoId",
                table: "Movimentacao",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacao_ProdutoId",
                table: "Movimentacao",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ajuste");

            migrationBuilder.DropTable(
                name: "Movimentacao");

            migrationBuilder.DropTable(
                name: "Caixa");
        }
    }
}
