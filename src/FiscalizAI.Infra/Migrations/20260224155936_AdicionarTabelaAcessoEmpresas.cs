using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiscalizAI.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarTabelaAcessoEmpresas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Uf",
                table: "Empresas",
                type: "char(2)",
                fixedLength: true,
                maxLength: 2,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldFixedLength: true,
                oldMaxLength: 2,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Uf",
                table: "Empresas",
                type: "char(2)",
                fixedLength: true,
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldFixedLength: true,
                oldMaxLength: 2);
        }
    }
}
