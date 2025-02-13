﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Software_Taller_y_Repuestos.Models;

#nullable disable

namespace Software_Taller_y_Repuestos.Migrations
{
    [DbContext(typeof(TallerRepuestosDbContext))]
    partial class TallerRepuestosDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ModelosAutoProducto", b =>
                {
                    b.Property<int>("ModelosModeloId")
                        .HasColumnType("int");

                    b.Property<int>("ProductosProductoId")
                        .HasColumnType("int");

                    b.HasKey("ModelosModeloId", "ProductosProductoId");

                    b.HasIndex("ProductosProductoId");

                    b.ToTable("ModelosAutoProducto");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.Carrito", b =>
                {
                    b.Property<int>("CarritoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CarritoId"));

                    b.Property<string>("ReciboPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuarioId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CarritoId");

                    b.ToTable("Carritos");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.CarritoItem", b =>
                {
                    b.Property<int>("CarritoItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CarritoItemId"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("CarritoId")
                        .HasColumnType("int");

                    b.Property<int>("ProductoId")
                        .HasColumnType("int");

                    b.HasKey("CarritoItemId");

                    b.HasIndex("CarritoId");

                    b.HasIndex("ProductoId");

                    b.ToTable("CarritoItems");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.Categoria", b =>
                {
                    b.Property<int>("CategoriaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CategoriaID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoriaId"));

                    b.Property<string>("Descripcion")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CategoriaId");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.DetalleFactura", b =>
                {
                    b.Property<int>("DetalleFacturaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DetalleFacturaId"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<string>("EstadoPago")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FacturaId")
                        .HasColumnType("int");

                    b.Property<string>("ImagenFactura")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PrecioUnitario")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductoId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductoId1")
                        .HasColumnType("int");

                    b.HasKey("DetalleFacturaId");

                    b.HasIndex("FacturaId");

                    b.HasIndex("ProductoId");

                    b.HasIndex("ProductoId1");

                    b.ToTable("DetalleFacturas");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.Factura", b =>
                {
                    b.Property<int>("FacturaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FacturaId"));

                    b.Property<DateTime>("FechaCompra")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("IVA")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Subtotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UsuarioId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UsuarioId1")
                        .HasColumnType("int");

                    b.HasKey("FacturaId");

                    b.HasIndex("UsuarioId1");

                    b.ToTable("Facturas");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.Horario", b =>
                {
                    b.Property<int>("HorarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("HorarioID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HorarioId"));

                    b.Property<decimal>("Ausencias")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime");

                    b.Property<decimal>("HorasExtras")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("HorasTrabajadas")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal>("Permisos")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int")
                        .HasColumnName("UsuarioID");

                    b.HasKey("HorarioId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Horarios");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.ModelosAuto", b =>
                {
                    b.Property<int>("ModeloId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ModeloID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ModeloId"));

                    b.Property<int>("Año")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ModeloId");

                    b.ToTable("ModelosAutos");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.PagosEmpleado", b =>
                {
                    b.Property<int>("PagoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("PagoID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PagoId"));

                    b.Property<decimal?>("Bonificaciones")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Deducciones")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("FechaPago")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<decimal?>("HorasExtras")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SalarioPagado")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int")
                        .HasColumnName("UsuarioID");

                    b.HasKey("PagoId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("PagosEmpleados");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.Producto", b =>
                {
                    b.Property<int>("ProductoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductoId"));

                    b.Property<bool>("Activo")
                        .HasColumnType("bit");

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int")
                        .HasColumnName("CategoriaID");

                    b.Property<string>("CategoriaNombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FacturaId")
                        .HasColumnType("int");

                    b.Property<string>("Imagen")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Marca")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("PrecioCompra")
                        .HasColumnType("decimal(12, 4)");

                    b.Property<decimal>("PrecioVenta")
                        .HasColumnType("decimal(12, 4)");

                    b.HasKey("ProductoId");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("Codigo")
                        .IsUnique();

                    b.HasIndex("FacturaId");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.Role", b =>
                {
                    b.Property<int>("RolId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RolId"));

                    b.Property<string>("NombreRol")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("RolId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            RolId = 1,
                            NombreRol = "Admin"
                        },
                        new
                        {
                            RolId = 2,
                            NombreRol = "Cliente"
                        },
                        new
                        {
                            RolId = 3,
                            NombreRol = "Empleado"
                        });
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.Usuario", b =>
                {
                    b.Property<int>("UsuarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UsuarioId"));

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Contrasenna")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Direccion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Estado")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("FechaIngreso")
                        .HasColumnType("datetime2");

                    b.Property<string>("Imagen")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RolId")
                        .HasColumnType("int")
                        .HasColumnName("RolID");

                    b.Property<decimal?>("SalarioBase")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Telefono")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UsuarioId");

                    b.HasIndex("Correo")
                        .IsUnique();

                    b.HasIndex("RolId");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("ModelosAutoProducto", b =>
                {
                    b.HasOne("Software_Taller_y_Repuestos.Models.ModelosAuto", null)
                        .WithMany()
                        .HasForeignKey("ModelosModeloId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Software_Taller_y_Repuestos.Models.Producto", null)
                        .WithMany()
                        .HasForeignKey("ProductosProductoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.CarritoItem", b =>
                {
                    b.HasOne("Software_Taller_y_Repuestos.Models.Carrito", "Carrito")
                        .WithMany("CarritoItems")
                        .HasForeignKey("CarritoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Software_Taller_y_Repuestos.Models.Producto", "Producto")
                        .WithMany()
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Carrito");

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.DetalleFactura", b =>
                {
                    b.HasOne("Software_Taller_y_Repuestos.Models.Factura", "Factura")
                        .WithMany("DetalleFacturas")
                        .HasForeignKey("FacturaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Software_Taller_y_Repuestos.Models.Producto", "Producto")
                        .WithMany()
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Software_Taller_y_Repuestos.Models.Producto", null)
                        .WithMany("DetallesFacturas")
                        .HasForeignKey("ProductoId1");

                    b.Navigation("Factura");

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.Factura", b =>
                {
                    b.HasOne("Software_Taller_y_Repuestos.Models.Usuario", null)
                        .WithMany("Facturas")
                        .HasForeignKey("UsuarioId1");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.Horario", b =>
                {
                    b.HasOne("Software_Taller_y_Repuestos.Models.Usuario", "Usuario")
                        .WithMany("Horarios")
                        .HasForeignKey("UsuarioId")
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.PagosEmpleado", b =>
                {
                    b.HasOne("Software_Taller_y_Repuestos.Models.Usuario", "Usuario")
                        .WithMany("PagosEmpleados")
                        .HasForeignKey("UsuarioId")
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.Producto", b =>
                {
                    b.HasOne("Software_Taller_y_Repuestos.Models.Categoria", "Categoria")
                        .WithMany("Productos")
                        .HasForeignKey("CategoriaId")
                        .IsRequired();

                    b.HasOne("Software_Taller_y_Repuestos.Models.Factura", null)
                        .WithMany("Productos")
                        .HasForeignKey("FacturaId");

                    b.Navigation("Categoria");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.Usuario", b =>
                {
                    b.HasOne("Software_Taller_y_Repuestos.Models.Role", "Rol")
                        .WithMany("Usuarios")
                        .HasForeignKey("RolId")
                        .IsRequired();

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.Carrito", b =>
                {
                    b.Navigation("CarritoItems");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.Categoria", b =>
                {
                    b.Navigation("Productos");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.Factura", b =>
                {
                    b.Navigation("DetalleFacturas");

                    b.Navigation("Productos");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.Producto", b =>
                {
                    b.Navigation("DetallesFacturas");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.Role", b =>
                {
                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("Software_Taller_y_Repuestos.Models.Usuario", b =>
                {
                    b.Navigation("Facturas");

                    b.Navigation("Horarios");

                    b.Navigation("PagosEmpleados");
                });
#pragma warning restore 612, 618
        }
    }
}
