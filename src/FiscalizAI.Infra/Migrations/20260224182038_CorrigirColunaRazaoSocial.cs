using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiscalizAI.Infra.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirColunaRazaoSocial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "varchar(200)",
                table: "Empresas",
                newName: "RazaoSocial");

            migrationBuilder.AlterColumn<string>(
                name: "RazaoSocial",
                table: "Empresas",
                type: "varchar(200)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RazaoSocial",
                table: "Empresas",
                newName: "varchar(200)");

            migrationBuilder.AlterColumn<string>(
                name: "varchar(200)",
                table: "Empresas",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 255);
        }
    }
}
