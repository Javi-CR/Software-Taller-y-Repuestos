using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Software_Taller_y_Repuestos.Migrations
{
    /// <inheritdoc />
    public partial class Final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "EstadoPago",
                table: "DetalleFacturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagenFactura",
                table: "DetalleFacturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReciboPath",
                table: "Carritos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoPago",
                table: "DetalleFacturas");

            migrationBuilder.DropColumn(
                name: "ImagenFactura",
                table: "DetalleFacturas");

            migrationBuilder.DropColumn(
                name: "ReciboPath",
                table: "Carritos");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
