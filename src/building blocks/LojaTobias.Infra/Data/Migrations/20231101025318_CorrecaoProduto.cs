using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LojaTobias.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class CorrecaoProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Embalagem",
                table: "ProdutoUnidade",
                type: "nvarchar(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)");

            migrationBuilder.AddColumn<decimal>(
                name: "PesoEmbalagem",
                table: "ProdutoUnidade",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantidade",
                table: "ProdutoUnidade",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "Removido",
                table: "Produto",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PesoEmbalagem",
                table: "ProdutoUnidade");

            migrationBuilder.DropColumn(
                name: "Quantidade",
                table: "ProdutoUnidade");

            migrationBuilder.DropColumn(
                name: "Removido",
                table: "Produto");

            migrationBuilder.AlterColumn<string>(
                name: "Embalagem",
                table: "ProdutoUnidade",
                type: "nvarchar(1000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldNullable: true);
        }
    }
}
