using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Software_Taller_y_Repuestos.Models;

public partial class GestionRepuestosDbContext : DbContext
{
    public GestionRepuestosDbContext()
    {
    }

    public GestionRepuestosDbContext(DbContextOptions<GestionRepuestosDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<DetallesFactura> DetallesFacturas { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<ModelosAuto> ModelosAutos { get; set; }

    public virtual DbSet<PagosEmpleado> PagosEmpleados { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=Localhost;Database=GestionRepuestosDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId).HasName("PK__Categori__F353C1C5CA0D64C1");

            entity.Property(e => e.CategoriaId).HasColumnName("CategoriaID");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Clientes__71ABD0A782CC2A08");

            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(50);
        });

        modelBuilder.Entity<DetallesFactura>(entity =>
        {
            entity.HasKey(e => e.DetalleId).HasName("PK__Detalles__6E19D6FA0BC4C588");

            entity.ToTable("DetallesFactura");

            entity.Property(e => e.DetalleId).HasColumnName("DetalleID");
            entity.Property(e => e.FacturaId).HasColumnName("FacturaID");
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");

            entity.HasOne(d => d.Factura).WithMany(p => p.DetallesFacturas)
                .HasForeignKey(d => d.FacturaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DetallesF__Factu__47DBAE45");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetallesFacturas)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DetallesF__Produ__48CFD27E");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.EmpleadoId).HasName("PK__Empleado__958BE6F057D186F2");

            entity.Property(e => e.EmpleadoId).HasColumnName("EmpleadoID");
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.FechaIngreso).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Puesto).HasMaxLength(100);
            entity.Property(e => e.SalarioBase).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Telefono).HasMaxLength(50);
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.FacturaId).HasName("PK__Facturas__5C02480531761184");

            entity.Property(e => e.FacturaId).HasColumnName("FacturaID");
            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.Descuento).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Impuestos).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Facturas__Client__44FF419A");
        });

        modelBuilder.Entity<ModelosAuto>(entity =>
        {
            entity.HasKey(e => e.ModeloId).HasName("PK__ModelosA__FA6052BA13789D74");

            entity.Property(e => e.ModeloId).HasColumnName("ModeloID");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<PagosEmpleado>(entity =>
        {
            entity.HasKey(e => e.PagoId).HasName("PK__PagosEmp__F00B61588EB2448A");

            entity.Property(e => e.PagoId).HasColumnName("PagoID");
            entity.Property(e => e.Bonificaciones).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Deducciones).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.EmpleadoId).HasColumnName("EmpleadoID");
            entity.Property(e => e.FechaPago)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.HorasExtras).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.SalarioPagado).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Empleado).WithMany(p => p.PagosEmpleados)
                .HasForeignKey(d => d.EmpleadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PagosEmpl__Emple__4E88ABD4");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("PK__Producto__A430AE83CBFB80C5");

            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");
            entity.Property(e => e.CategoriaId).HasColumnName("CategoriaID");
            entity.Property(e => e.Codigo).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.PrecioCompra).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PrecioVenta).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Productos__Categ__398D8EEE");

            entity.HasMany(d => d.Modelos).WithMany(p => p.Productos)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductosModelo",
                    r => r.HasOne<ModelosAuto>().WithMany()
                        .HasForeignKey("ModeloId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Productos__Model__3F466844"),
                    l => l.HasOne<Producto>().WithMany()
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Productos__Produ__3E52440B"),
                    j =>
                    {
                        j.HasKey("ProductoId", "ModeloId").HasName("PK__Producto__1B96ABA8CE3F11F4");
                        j.ToTable("ProductosModelos");
                        j.IndexerProperty<int>("ProductoId").HasColumnName("ProductoID");
                        j.IndexerProperty<int>("ModeloId").HasColumnName("ModeloID");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
