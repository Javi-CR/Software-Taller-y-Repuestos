using System;
using Microsoft.EntityFrameworkCore;

namespace Software_Taller_y_Repuestos.Models
{
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
        public virtual DbSet<Horario> Horarios { get; set; }
        public virtual DbSet<ModelosAuto> ModelosAutos { get; set; }
        public virtual DbSet<PagosEmpleado> PagosEmpleados { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<CarritoItem> CarritoItems { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<DetalleFactura> DetalleFacturas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Leer la cadena de conexión desde appsettings.json
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:TallerRepuestosDb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureCategorias(modelBuilder);
            ConfigureHorarios(modelBuilder);
            ConfigureModelosAuto(modelBuilder);
            ConfigurePagosEmpleado(modelBuilder);
            ConfigureProductos(modelBuilder);
            ConfigureCarritos(modelBuilder);
            ConfigureFacturas(modelBuilder);
            ConfigureRoles(modelBuilder);
            ConfigureUsuarios(modelBuilder);

            OnModelCreatingPartial(modelBuilder);
        }

        private static void ConfigureCategorias(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.CategoriaId);
                entity.Property(e => e.CategoriaId).HasColumnName("CategoriaID");
                entity.Property(e => e.Nombre).HasMaxLength(100);
                entity.Property(e => e.Descripcion).HasMaxLength(255);
            });
        }

       
        private static void ConfigureHorarios(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Horario>(entity =>
            {
                entity.HasKey(e => e.HorarioId);
                entity.Property(e => e.HorarioId).HasColumnName("HorarioID");
                entity.Property(e => e.Fecha).HasColumnType("datetime");
                entity.Property(e => e.HorasTrabajadas).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Horarios)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }

        private static void ConfigureModelosAuto(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ModelosAuto>(entity =>
            {
                entity.HasKey(e => e.ModeloId);
                entity.Property(e => e.ModeloId).HasColumnName("ModeloID");
                entity.Property(e => e.Nombre).HasMaxLength(100);
            });
        }

        private static void ConfigurePagosEmpleado(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PagosEmpleado>(entity =>
            {
                entity.HasKey(e => e.PagoId);
                entity.Property(e => e.PagoId).HasColumnName("PagoID");
                entity.Property(e => e.FechaPago).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.PagosEmpleados)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }

        private static void ConfigureProductos(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.ProductoId);
                entity.HasIndex(e => e.Codigo).IsUnique();

                entity.Property(e => e.CategoriaId).HasColumnName("CategoriaID");
                entity.Property(e => e.Nombre).HasMaxLength(100);
                entity.Property(e => e.PrecioCompra).HasColumnType("decimal(12, 4)");
                entity.Property(e => e.PrecioVenta).HasColumnType("decimal(12, 4)");

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.CategoriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }

        private static void ConfigureCarritos(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Carrito>()
                .HasMany(c => c.CarritoItems)
                .WithOne(ci => ci.Carrito)
                .HasForeignKey(ci => ci.CarritoId);
        }

        private static void ConfigureFacturas(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Factura>()
                .HasMany(f => f.DetalleFacturas)
                .WithOne(df => df.Factura)
                .HasForeignKey(df => df.FacturaId);

            modelBuilder.Entity<DetalleFactura>()
                .HasOne(df => df.Producto)
                .WithMany()
                .HasForeignKey(df => df.ProductoId);
        }

        private static void ConfigureRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RolId);
                entity.Property(e => e.NombreRol).HasMaxLength(50);

                entity.HasData(
                    new Role { RolId = 1, NombreRol = "Admin" },
                    new Role { RolId = 2, NombreRol = "Cliente" },
                    new Role { RolId = 3, NombreRol = "Empleado" }
                );
            });
        }

        private static void ConfigureUsuarios(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.UsuarioId);
                entity.HasIndex(e => e.Correo).IsUnique();

                entity.Property(e => e.Correo).HasMaxLength(100);
                entity.Property(e => e.RolId).HasColumnName("RolID");

                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.RolId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
