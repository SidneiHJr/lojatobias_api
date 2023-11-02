using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LojaTobias.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterUnidadeMedidaAddRemovido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Removido",
                table: "UnidadeMedida",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Produto",
                type: "nvarchar(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Removido",
                table: "UnidadeMedida");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Produto",
                type: "nvarchar(1000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldNullable: true);
        }
    }
}
