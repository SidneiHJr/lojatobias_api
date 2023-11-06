using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LojaTobias.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterAjusteCamposNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ajuste_Movimentacao_MovimentacaoId",
                table: "Ajuste");

            migrationBuilder.DropIndex(
                name: "IX_Ajuste_MovimentacaoId",
                table: "Ajuste");

            migrationBuilder.DropColumn(
                name: "MovimentacaoId",
                table: "Ajuste");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacao_AjusteId",
                table: "Movimentacao",
                column: "AjusteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movimentacao_Ajuste_AjusteId",
                table: "Movimentacao",
                column: "AjusteId",
                principalTable: "Ajuste",
                principalColumn: "AjusteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movimentacao_Ajuste_AjusteId",
                table: "Movimentacao");

            migrationBuilder.DropIndex(
                name: "IX_Movimentacao_AjusteId",
                table: "Movimentacao");

            migrationBuilder.AddColumn<Guid>(
                name: "MovimentacaoId",
                table: "Ajuste",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Ajuste_MovimentacaoId",
                table: "Ajuste",
                column: "MovimentacaoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ajuste_Movimentacao_MovimentacaoId",
                table: "Ajuste",
                column: "MovimentacaoId",
                principalTable: "Movimentacao",
                principalColumn: "MovimentacaoId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
