using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Software_Taller_y_Repuestos.Migrations
{
    /// <inheritdoc />
    public partial class carrito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Facturas__Usuari__4AB81AF0",
                table: "Facturas");

            migrationBuilder.DropForeignKey(
                name: "FK__Horarios__Usuari__5812160E",
                table: "Horarios");

            migrationBuilder.DropForeignKey(
                name: "FK__PagosEmpl__Usuar__52593CB8",
                table: "PagosEmpleados");

            migrationBuilder.DropForeignKey(
                name: "FK__Productos__Categ__3A81B327",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK__Usuarios__RolID__45F365D3",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "CodigosDescuento");

            migrationBuilder.DropTable(
                name: "CuentaBancaria");

            migrationBuilder.DropTable(
                name: "DetallesFactura");

            migrationBuilder.DropTable(
                name: "ProductosModelos");

            migrationBuilder.DropTable(
                name: "ProveedoresProductos");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Usuarios__2B3DE798CBEB8A18",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Roles__F92302D1DDE5C884",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Producto__A430AE8382118DC5",
                table: "Productos");

            migrationBuilder.DropPrimaryKey(
                name: "PK__PagosEmp__F00B6158767EE0C1",
                table: "PagosEmpleados");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ModelosA__FA6052BA4C821877",
                table: "ModelosAutos");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Horarios__BB881A9E0A46E376",
                table: "Horarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Facturas__5C02480534443468",
                table: "Facturas");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_UsuarioID",
                table: "Facturas");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Categori__F353C1C57A85E13B",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "CorreoEnviado",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "Descuento",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "Impuestos",
                table: "Facturas");

            migrationBuilder.RenameColumn(
                name: "UsuarioID",
                table: "Usuarios",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "UQ__Usuarios__60695A19AF60258D",
                table: "Usuarios",
                newName: "IX_Usuarios_Correo");

            migrationBuilder.RenameColumn(
                name: "RolID",
                table: "Roles",
                newName: "RolId");

            migrationBuilder.RenameColumn(
                name: "ProductoID",
                table: "Productos",
                newName: "ProductoId");

            migrationBuilder.RenameIndex(
                name: "UQ__Producto__06370DAC3327131C",
                table: "Productos",
                newName: "IX_Productos_Codigo");

            migrationBuilder.RenameColumn(
                name: "UsuarioID",
                table: "Facturas",
                newName: "UsuarioId");

            migrationBuilder.RenameColumn(
                name: "FacturaID",
                table: "Facturas",
                newName: "FacturaId");

            migrationBuilder.RenameColumn(
                name: "CodigoDescuentoID",
                table: "Facturas",
                newName: "UsuarioId1");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SalarioBase",
                table: "Usuarios",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Imagen",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaIngreso",
                table: "Usuarios",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Marca",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Imagen",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "Productos",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<decimal>(
                name: "SalarioPagado",
                table: "PagosEmpleados",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "HorasExtras",
                table: "PagosEmpleados",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Deducciones",
                table: "PagosEmpleados",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Bonificaciones",
                table: "PagosEmpleados",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Permisos",
                table: "Horarios",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "HorasExtras",
                table: "Horarios",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ausencias",
                table: "Horarios",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Facturas",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCompra",
                table: "Facturas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "IVA",
                table: "Facturas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal",
                table: "Facturas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios",
                column: "UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "RolId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Productos",
                table: "Productos",
                column: "ProductoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PagosEmpleados",
                table: "PagosEmpleados",
                column: "PagoID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModelosAutos",
                table: "ModelosAutos",
                column: "ModeloID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Horarios",
                table: "Horarios",
                column: "HorarioID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Facturas",
                table: "Facturas",
                column: "FacturaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categorias",
                table: "Categorias",
                column: "CategoriaID");

            migrationBuilder.CreateTable(
                name: "Carritos",
                columns: table => new
                {
                    CarritoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carritos", x => x.CarritoId);
                });

            migrationBuilder.CreateTable(
                name: "DetalleFacturas",
                columns: table => new
                {
                    DetalleFacturaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductoId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleFacturas", x => x.DetalleFacturaId);
                    table.ForeignKey(
                        name: "FK_DetalleFacturas_Facturas_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "Facturas",
                        principalColumn: "FacturaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleFacturas_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "ProductoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleFacturas_Productos_ProductoId1",
                        column: x => x.ProductoId1,
                        principalTable: "Productos",
                        principalColumn: "ProductoId");
                });

            migrationBuilder.CreateTable(
                name: "ModelosAutoProducto",
                columns: table => new
                {
                    ModelosModeloId = table.Column<int>(type: "int", nullable: false),
                    ProductosProductoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelosAutoProducto", x => new { x.ModelosModeloId, x.ProductosProductoId });
                    table.ForeignKey(
                        name: "FK_ModelosAutoProducto_ModelosAutos_ModelosModeloId",
                        column: x => x.ModelosModeloId,
                        principalTable: "ModelosAutos",
                        principalColumn: "ModeloID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelosAutoProducto_Productos_ProductosProductoId",
                        column: x => x.ProductosProductoId,
                        principalTable: "Productos",
                        principalColumn: "ProductoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarritoItems",
                columns: table => new
                {
                    CarritoItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarritoId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarritoItems", x => x.CarritoItemId);
                    table.ForeignKey(
                        name: "FK_CarritoItems_Carritos_CarritoId",
                        column: x => x.CarritoId,
                        principalTable: "Carritos",
                        principalColumn: "CarritoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarritoItems_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "ProductoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_UsuarioId1",
                table: "Facturas",
                column: "UsuarioId1");

            migrationBuilder.CreateIndex(
                name: "IX_CarritoItems_CarritoId",
                table: "CarritoItems",
                column: "CarritoId");

            migrationBuilder.CreateIndex(
                name: "IX_CarritoItems_ProductoId",
                table: "CarritoItems",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleFacturas_FacturaId",
                table: "DetalleFacturas",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleFacturas_ProductoId",
                table: "DetalleFacturas",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleFacturas_ProductoId1",
                table: "DetalleFacturas",
                column: "ProductoId1");

            migrationBuilder.CreateIndex(
                name: "IX_ModelosAutoProducto_ProductosProductoId",
                table: "ModelosAutoProducto",
                column: "ProductosProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_Usuarios_UsuarioId1",
                table: "Facturas",
                column: "UsuarioId1",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Horarios_Usuarios_UsuarioID",
                table: "Horarios",
                column: "UsuarioID",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_PagosEmpleados_Usuarios_UsuarioID",
                table: "PagosEmpleados",
                column: "UsuarioID",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Categorias_CategoriaID",
                table: "Productos",
                column: "CategoriaID",
                principalTable: "Categorias",
                principalColumn: "CategoriaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Roles_RolID",
                table: "Usuarios",
                column: "RolID",
                principalTable: "Roles",
                principalColumn: "RolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_Usuarios_UsuarioId1",
                table: "Facturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Horarios_Usuarios_UsuarioID",
                table: "Horarios");

            migrationBuilder.DropForeignKey(
                name: "FK_PagosEmpleados_Usuarios_UsuarioID",
                table: "PagosEmpleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Categorias_CategoriaID",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Roles_RolID",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "CarritoItems");

            migrationBuilder.DropTable(
                name: "DetalleFacturas");

            migrationBuilder.DropTable(
                name: "ModelosAutoProducto");

            migrationBuilder.DropTable(
                name: "Carritos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Productos",
                table: "Productos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PagosEmpleados",
                table: "PagosEmpleados");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModelosAutos",
                table: "ModelosAutos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Horarios",
                table: "Horarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Facturas",
                table: "Facturas");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_UsuarioId1",
                table: "Facturas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categorias",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "FechaCompra",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "IVA",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "Facturas");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Usuarios",
                newName: "UsuarioID");

            migrationBuilder.RenameIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios",
                newName: "UQ__Usuarios__60695A19AF60258D");

            migrationBuilder.RenameColumn(
                name: "RolId",
                table: "Roles",
                newName: "RolID");

            migrationBuilder.RenameColumn(
                name: "ProductoId",
                table: "Productos",
                newName: "ProductoID");

            migrationBuilder.RenameIndex(
                name: "IX_Productos_Codigo",
                table: "Productos",
                newName: "UQ__Producto__06370DAC3327131C");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Facturas",
                newName: "UsuarioID");

            migrationBuilder.RenameColumn(
                name: "FacturaId",
                table: "Facturas",
                newName: "FacturaID");

            migrationBuilder.RenameColumn(
                name: "UsuarioId1",
                table: "Facturas",
                newName: "CodigoDescuentoID");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Usuarios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SalarioBase",
                table: "Usuarios",
                type: "decimal(12,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Usuarios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Imagen",
                table: "Usuarios",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaIngreso",
                table: "Usuarios",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Usuarios",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Marca",
                table: "Productos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Imagen",
                table: "Productos",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Productos",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "Productos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SalarioPagado",
                table: "PagosEmpleados",
                type: "decimal(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "HorasExtras",
                table: "PagosEmpleados",
                type: "decimal(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Deducciones",
                table: "PagosEmpleados",
                type: "decimal(12,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Bonificaciones",
                table: "PagosEmpleados",
                type: "decimal(12,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Permisos",
                table: "Horarios",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "HorasExtras",
                table: "Horarios",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ausencias",
                table: "Horarios",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioID",
                table: "Facturas",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Facturas",
                type: "decimal(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<bool>(
                name: "CorreoEnviado",
                table: "Facturas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Descuento",
                table: "Facturas",
                type: "decimal(12,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha",
                table: "Facturas",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<decimal>(
                name: "Impuestos",
                table: "Facturas",
                type: "decimal(12,2)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__Usuarios__2B3DE798CBEB8A18",
                table: "Usuarios",
                column: "UsuarioID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Roles__F92302D1DDE5C884",
                table: "Roles",
                column: "RolID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Producto__A430AE8382118DC5",
                table: "Productos",
                column: "ProductoID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__PagosEmp__F00B6158767EE0C1",
                table: "PagosEmpleados",
                column: "PagoID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__ModelosA__FA6052BA4C821877",
                table: "ModelosAutos",
                column: "ModeloID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Horarios__BB881A9E0A46E376",
                table: "Horarios",
                column: "HorarioID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Facturas__5C02480534443468",
                table: "Facturas",
                column: "FacturaID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Categori__F353C1C57A85E13B",
                table: "Categorias",
                column: "CategoriaID");

            migrationBuilder.CreateTable(
                name: "CodigosDescuento",
                columns: table => new
                {
                    CodigoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime", nullable: true),
                    TipoDescuento = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ValorDescuento = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CodigosD__DB319CF3407E9E87", x => x.CodigoID);
                });

            migrationBuilder.CreateTable(
                name: "CuentaBancaria",
                columns: table => new
                {
                    CuentaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    Banco = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumeroCuenta = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TipoCuenta = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CuentaBa__40072EA196982E58", x => x.CuentaID);
                    table.ForeignKey(
                        name: "FK__CuentaBan__Usuar__5BE2A6F2",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "DetallesFactura",
                columns: table => new
                {
                    DetalleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacturaID = table.Column<int>(type: "int", nullable: false),
                    ProductoID = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Detalles__6E19D6FA0DD8E7EA", x => x.DetalleID);
                    table.ForeignKey(
                        name: "FK__DetallesF__Factu__4D94879B",
                        column: x => x.FacturaID,
                        principalTable: "Facturas",
                        principalColumn: "FacturaID");
                    table.ForeignKey(
                        name: "FK__DetallesF__Produ__4E88ABD4",
                        column: x => x.ProductoID,
                        principalTable: "Productos",
                        principalColumn: "ProductoID");
                });

            migrationBuilder.CreateTable(
                name: "ProductosModelos",
                columns: table => new
                {
                    ProductoID = table.Column<int>(type: "int", nullable: false),
                    ModeloID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Producto__1B96ABA86357B125", x => new { x.ProductoID, x.ModeloID });
                    table.ForeignKey(
                        name: "FK__Productos__Model__403A8C7D",
                        column: x => x.ModeloID,
                        principalTable: "ModelosAutos",
                        principalColumn: "ModeloID");
                    table.ForeignKey(
                        name: "FK__Productos__Produ__3F466844",
                        column: x => x.ProductoID,
                        principalTable: "Productos",
                        principalColumn: "ProductoID");
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    ProveedorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RUC = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Proveedo__61266A59B2C75164", x => x.ProveedorId);
                });

            migrationBuilder.CreateTable(
                name: "ProveedoresProductos",
                columns: table => new
                {
                    ProveedorId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Proveedo__4B6560B3A9D5AFBE", x => new { x.ProveedorId, x.ProductoId });
                    table.ForeignKey(
                        name: "FK__Proveedor__Produ__6477ECF3",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "ProductoID");
                    table.ForeignKey(
                        name: "FK__Proveedor__Prove__6383C8BA",
                        column: x => x.ProveedorId,
                        principalTable: "Proveedores",
                        principalColumn: "ProveedorId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_UsuarioID",
                table: "Facturas",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_CuentaBancaria_UsuarioID",
                table: "CuentaBancaria",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesFactura_FacturaID",
                table: "DetallesFactura",
                column: "FacturaID");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesFactura_ProductoID",
                table: "DetallesFactura",
                column: "ProductoID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosModelos_ModeloID",
                table: "ProductosModelos",
                column: "ModeloID");

            migrationBuilder.CreateIndex(
                name: "IX_ProveedoresProductos_ProductoId",
                table: "ProveedoresProductos",
                column: "ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK__Facturas__Usuari__4AB81AF0",
                table: "Facturas",
                column: "UsuarioID",
                principalTable: "Usuarios",
                principalColumn: "UsuarioID");

            migrationBuilder.AddForeignKey(
                name: "FK__Horarios__Usuari__5812160E",
                table: "Horarios",
                column: "UsuarioID",
                principalTable: "Usuarios",
                principalColumn: "UsuarioID");

            migrationBuilder.AddForeignKey(
                name: "FK__PagosEmpl__Usuar__52593CB8",
                table: "PagosEmpleados",
                column: "UsuarioID",
                principalTable: "Usuarios",
                principalColumn: "UsuarioID");

            migrationBuilder.AddForeignKey(
                name: "FK__Productos__Categ__3A81B327",
                table: "Productos",
                column: "CategoriaID",
                principalTable: "Categorias",
                principalColumn: "CategoriaID");

            migrationBuilder.AddForeignKey(
                name: "FK__Usuarios__RolID__45F365D3",
                table: "Usuarios",
                column: "RolID",
                principalTable: "Roles",
                principalColumn: "RolID");
        }
    }
}
