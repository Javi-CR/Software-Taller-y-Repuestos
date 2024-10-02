using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Software_Taller_y_Repuestos.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    CategoriaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__F353C1C57A85E13B", x => x.CategoriaID);
                });

            migrationBuilder.CreateTable(
                name: "CodigosDescuento",
                columns: table => new
                {
                    CodigoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ValorDescuento = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    TipoDescuento = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CodigosD__DB319CF3407E9E87", x => x.CodigoID);
                });

            migrationBuilder.CreateTable(
                name: "ModelosAutos",
                columns: table => new
                {
                    ModeloID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Año = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ModelosA__FA6052BA4C821877", x => x.ModeloID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RolID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreRol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Roles__F92302D1DDE5C884", x => x.RolID);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    ProductoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategoriaID = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioCompra = table.Column<decimal>(type: "decimal(12,4)", nullable: false),
                    PrecioVenta = table.Column<decimal>(type: "decimal(12,4)", nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Imagen = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Producto__A430AE8382118DC5", x => x.ProductoID);
                    table.ForeignKey(
                        name: "FK__Productos__Categ__3A81B327",
                        column: x => x.CategoriaID,
                        principalTable: "Categorias",
                        principalColumn: "CategoriaID");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RolID = table.Column<int>(type: "int", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime", nullable: true),
                    SalarioBase = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Imagen = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuarios__2B3DE798CBEB8A18", x => x.UsuarioID);
                    table.ForeignKey(
                        name: "FK__Usuarios__RolID__45F365D3",
                        column: x => x.RolID,
                        principalTable: "Roles",
                        principalColumn: "RolID");
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
                name: "Facturas",
                columns: table => new
                {
                    FacturaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Total = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Impuestos = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Descuento = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    CorreoEnviado = table.Column<bool>(type: "bit", nullable: false),
                    CodigoDescuentoID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Facturas__5C02480534443468", x => x.FacturaID);
                    table.ForeignKey(
                        name: "FK__Facturas__Usuari__4AB81AF0",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "Horarios",
                columns: table => new
                {
                    HorarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    HorasTrabajadas = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    HorasExtras = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Ausencias = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Permisos = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Horarios__BB881A9E0A46E376", x => x.HorarioID);
                    table.ForeignKey(
                        name: "FK__Horarios__Usuari__5812160E",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "PagosEmpleados",
                columns: table => new
                {
                    PagoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    FechaPago = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    SalarioPagado = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    HorasExtras = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Deducciones = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Bonificaciones = table.Column<decimal>(type: "decimal(12,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PagosEmp__F00B6158767EE0C1", x => x.PagoID);
                    table.ForeignKey(
                        name: "FK__PagosEmpl__Usuar__52593CB8",
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
                name: "IX_Facturas_UsuarioID",
                table: "Facturas",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_UsuarioID",
                table: "Horarios",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_PagosEmpleados_UsuarioID",
                table: "PagosEmpleados",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CategoriaID",
                table: "Productos",
                column: "CategoriaID");

            migrationBuilder.CreateIndex(
                name: "UQ__Producto__06370DAC3327131C",
                table: "Productos",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductosModelos_ModeloID",
                table: "ProductosModelos",
                column: "ModeloID");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolID",
                table: "Usuarios",
                column: "RolID");

            migrationBuilder.CreateIndex(
                name: "UQ__Usuarios__60695A19AF60258D",
                table: "Usuarios",
                column: "Correo",
                unique: true,
                filter: "[Correo] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodigosDescuento");

            migrationBuilder.DropTable(
                name: "CuentaBancaria");

            migrationBuilder.DropTable(
                name: "DetallesFactura");

            migrationBuilder.DropTable(
                name: "Horarios");

            migrationBuilder.DropTable(
                name: "PagosEmpleados");

            migrationBuilder.DropTable(
                name: "ProductosModelos");

            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.DropTable(
                name: "ModelosAutos");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
