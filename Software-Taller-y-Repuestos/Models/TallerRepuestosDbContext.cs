using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Software_Taller_y_Repuestos.Models;

public partial class TallerRepuestosDbContext : DbContext
{
    public TallerRepuestosDbContext()
    {
    }

    public TallerRepuestosDbContext(DbContextOptions<TallerRepuestosDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<CodigosDescuento> CodigosDescuentos { get; set; }

    public virtual DbSet<CuentaBancarium> CuentaBancaria { get; set; }

    public virtual DbSet<DetallesFactura> DetallesFacturas { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<ModelosAuto> ModelosAutos { get; set; }

    public virtual DbSet<PagosEmpleado> PagosEmpleados { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=TallerRepuestosDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId).HasName("PK__Categori__F353C1C57A85E13B");

            entity.Property(e => e.CategoriaId).HasColumnName("CategoriaID");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<CodigosDescuento>(entity =>
        {
            entity.HasKey(e => e.CodigoId).HasName("PK__CodigosD__DB319CF3407E9E87");

            entity.ToTable("CodigosDescuento");

            entity.Property(e => e.CodigoId).HasColumnName("CodigoID");
            entity.Property(e => e.Codigo).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.FechaExpiracion).HasColumnType("datetime");
            entity.Property(e => e.TipoDescuento).HasMaxLength(10);
            entity.Property(e => e.ValorDescuento).HasColumnType("decimal(12, 2)");
        });

        modelBuilder.Entity<CuentaBancarium>(entity =>
        {
            entity.HasKey(e => e.CuentaId).HasName("PK__CuentaBa__40072EA196982E58");

            entity.Property(e => e.CuentaId).HasColumnName("CuentaID");
            entity.Property(e => e.Banco).HasMaxLength(100);
            entity.Property(e => e.NumeroCuenta).HasMaxLength(50);
            entity.Property(e => e.TipoCuenta).HasMaxLength(50);
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.CuentaBancaria)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CuentaBan__Usuar__5BE2A6F2");
        });

        modelBuilder.Entity<DetallesFactura>(entity =>
        {
            entity.HasKey(e => e.DetalleId).HasName("PK__Detalles__6E19D6FA0DD8E7EA");

            entity.ToTable("DetallesFactura");

            entity.Property(e => e.DetalleId).HasColumnName("DetalleID");
            entity.Property(e => e.FacturaId).HasColumnName("FacturaID");
            entity.Property(e => e.Precio).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");

            entity.HasOne(d => d.Factura).WithMany(p => p.DetallesFacturas)
                .HasForeignKey(d => d.FacturaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DetallesF__Factu__4D94879B");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetallesFacturas)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DetallesF__Produ__4E88ABD4");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.FacturaId).HasName("PK__Facturas__5C02480534443468");

            entity.Property(e => e.FacturaId).HasColumnName("FacturaID");
            entity.Property(e => e.CodigoDescuentoId).HasColumnName("CodigoDescuentoID");
            entity.Property(e => e.Descuento).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Impuestos).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Facturas__Usuari__4AB81AF0");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.HorarioId).HasName("PK__Horarios__BB881A9E0A46E376");

            entity.Property(e => e.HorarioId).HasColumnName("HorarioID");
            entity.Property(e => e.Ausencias).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.HorasExtras).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.HorasTrabajadas).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Permisos).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Horarios)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Horarios__Usuari__5812160E");
        });

        modelBuilder.Entity<ModelosAuto>(entity =>
        {
            entity.HasKey(e => e.ModeloId).HasName("PK__ModelosA__FA6052BA4C821877");

            entity.Property(e => e.ModeloId).HasColumnName("ModeloID");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<PagosEmpleado>(entity =>
        {
            entity.HasKey(e => e.PagoId).HasName("PK__PagosEmp__F00B6158767EE0C1");

            entity.Property(e => e.PagoId).HasColumnName("PagoID");
            entity.Property(e => e.Bonificaciones).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Deducciones).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.FechaPago)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.HorasExtras).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.SalarioPagado).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.PagosEmpleados)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PagosEmpl__Usuar__52593CB8");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("PK__Producto__A430AE8382118DC5");

            entity.HasIndex(e => e.Codigo, "UQ__Producto__06370DAC3327131C").IsUnique();

            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");
            entity.Property(e => e.CategoriaId).HasColumnName("CategoriaID");
            entity.Property(e => e.Codigo).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Imagen).HasMaxLength(255);
            entity.Property(e => e.Marca).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.PrecioCompra).HasColumnType("decimal(12, 4)");
            entity.Property(e => e.PrecioVenta).HasColumnType("decimal(12, 4)");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Productos__Categ__3A81B327");

            entity.HasMany(d => d.Modelos).WithMany(p => p.Productos)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductosModelo",
                    r => r.HasOne<ModelosAuto>().WithMany()
                        .HasForeignKey("ModeloId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Productos__Model__403A8C7D"),
                    l => l.HasOne<Producto>().WithMany()
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Productos__Produ__3F466844"),
                    j =>
                    {
                        j.HasKey("ProductoId", "ModeloId").HasName("PK__Producto__1B96ABA86357B125");
                        j.ToTable("ProductosModelos");
                        j.IndexerProperty<int>("ProductoId").HasColumnName("ProductoID");
                        j.IndexerProperty<int>("ModeloId").HasColumnName("ModeloID");
                    });
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.ProveedorId).HasName("PK__Proveedo__61266A59B2C75164");

            entity.Property(e => e.Direccion).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Ruc)
                .HasMaxLength(20)
                .HasColumnName("RUC");
            entity.Property(e => e.Telefono).HasMaxLength(20);

            entity.HasMany(d => d.Productos).WithMany(p => p.Proveedors)
                .UsingEntity<Dictionary<string, object>>(
                    "ProveedoresProducto",
                    r => r.HasOne<Producto>().WithMany()
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Proveedor__Produ__6477ECF3"),
                    l => l.HasOne<Proveedore>().WithMany()
                        .HasForeignKey("ProveedorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Proveedor__Prove__6383C8BA"),
                    j =>
                    {
                        j.HasKey("ProveedorId", "ProductoId").HasName("PK__Proveedo__4B6560B3A9D5AFBE");
                        j.ToTable("ProveedoresProductos");
                    });
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__Roles__F92302D1DDE5C884");

            entity.Property(e => e.RolId).HasColumnName("RolID");
            entity.Property(e => e.NombreRol).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE798CBEB8A18");

            entity.HasIndex(e => e.Correo, "UQ__Usuarios__60695A19AF60258D").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.FechaIngreso).HasColumnType("datetime");
            entity.Property(e => e.Imagen).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.RolId).HasColumnName("RolID");
            entity.Property(e => e.SalarioBase).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Telefono).HasMaxLength(50);

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuarios__RolID__45F365D3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
