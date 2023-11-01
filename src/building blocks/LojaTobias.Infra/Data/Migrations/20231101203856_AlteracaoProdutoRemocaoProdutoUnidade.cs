using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LojaTobias.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlteracaoProdutoRemocaoProdutoUnidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdutoUnidade");

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Produto",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Quantidade",
                table: "Produto",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "UnidadeMedidaId",
                table: "Produto",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Produto_UnidadeMedidaId",
                table: "Produto",
                column: "UnidadeMedidaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_UnidadeMedida_UnidadeMedidaId",
                table: "Produto",
                column: "UnidadeMedidaId",
                principalTable: "UnidadeMedida",
                principalColumn: "UnidadeMedidaId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produto_UnidadeMedida_UnidadeMedidaId",
                table: "Produto");

            migrationBuilder.DropIndex(
                name: "IX_Produto_UnidadeMedidaId",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "Quantidade",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "UnidadeMedidaId",
                table: "Produto");

            migrationBuilder.CreateTable(
                name: "ProdutoUnidade",
                columns: table => new
                {
                    ProdutoUnidadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Embalagem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PesoEmbalagem = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProdutoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnidadeMedidaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioAtualizacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioCriacao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoUnidade", x => x.ProdutoUnidadeId);
                    table.ForeignKey(
                        name: "FK_ProdutoUnidade_Produto_ProdutoUnidadeId",
                        column: x => x.ProdutoUnidadeId,
                        principalTable: "Produto",
                        principalColumn: "ProdutoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutoUnidade_UnidadeMedida_ProdutoUnidadeId",
                        column: x => x.ProdutoUnidadeId,
                        principalTable: "UnidadeMedida",
                        principalColumn: "UnidadeMedidaId",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
