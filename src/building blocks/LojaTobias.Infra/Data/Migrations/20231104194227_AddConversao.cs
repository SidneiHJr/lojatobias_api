using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LojaTobias.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddConversao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UnidadeMedidaId",
                table: "PedidoItem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "UnidadeMedidaConversao",
                columns: table => new
                {
                    UnidadeMedidaConversaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FatorConversao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnidadeMedidaEntradaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnidadeMedidaSaidaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCriacao = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioAtualizacao = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadeMedidaConversao", x => x.UnidadeMedidaConversaoId);
                    table.ForeignKey(
                        name: "FK_UnidadeMedidaConversao_UnidadeMedida_UnidadeMedidaEntradaId",
                        column: x => x.UnidadeMedidaEntradaId,
                        principalTable: "UnidadeMedida",
                        principalColumn: "UnidadeMedidaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnidadeMedidaConversao_UnidadeMedida_UnidadeMedidaSaidaId",
                        column: x => x.UnidadeMedidaSaidaId,
                        principalTable: "UnidadeMedida",
                        principalColumn: "UnidadeMedidaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItem_UnidadeMedidaId",
                table: "PedidoItem",
                column: "UnidadeMedidaId");

            migrationBuilder.CreateIndex(
                name: "IX_UnidadeMedidaConversao_UnidadeMedidaEntradaId",
                table: "UnidadeMedidaConversao",
                column: "UnidadeMedidaEntradaId");

            migrationBuilder.CreateIndex(
                name: "IX_UnidadeMedidaConversao_UnidadeMedidaSaidaId",
                table: "UnidadeMedidaConversao",
                column: "UnidadeMedidaSaidaId");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoItem_UnidadeMedida_UnidadeMedidaId",
                table: "PedidoItem",
                column: "UnidadeMedidaId",
                principalTable: "UnidadeMedida",
                principalColumn: "UnidadeMedidaId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoItem_UnidadeMedida_UnidadeMedidaId",
                table: "PedidoItem");

            migrationBuilder.DropTable(
                name: "UnidadeMedidaConversao");

            migrationBuilder.DropIndex(
                name: "IX_PedidoItem_UnidadeMedidaId",
                table: "PedidoItem");

            migrationBuilder.DropColumn(
                name: "UnidadeMedidaId",
                table: "PedidoItem");
        }
    }
}
