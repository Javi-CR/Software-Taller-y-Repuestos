using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Software_Taller_y_Repuestos.Migrations
{
    /// <inheritdoc />
    public partial class factura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FacturaId",
                table: "Productos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_FacturaId",
                table: "Productos",
                column: "FacturaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Facturas_FacturaId",
                table: "Productos",
                column: "FacturaId",
                principalTable: "Facturas",
                principalColumn: "FacturaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Facturas_FacturaId",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_FacturaId",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "FacturaId",
                table: "Productos");
        }
    }
}
