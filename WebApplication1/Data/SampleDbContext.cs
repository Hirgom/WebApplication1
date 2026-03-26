using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public partial class SampleDbContext : DbContext
{
    public SampleDbContext()
    {
    }

    public SampleDbContext(DbContextOptions<SampleDbContext> options)
        : base(options)
    {
    }

    // DbSets (plural) matching your database tables
    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<Product> Products { get; set; } = null!;
    public virtual DbSet<Proveedor> Proveedores { get; set; } = null!;
    public virtual DbSet<MovimientoInventario> MovimientosInventario { get; set; } = null!;
    public virtual DbSet<Venta> Ventas { get; set; } = null!;
    public virtual DbSet<DetalleVenta> DetalleVentas { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Keep this guarded so Program.cs AddDbContext controls configuration
        if (!optionsBuilder.IsConfigured)
        {
#warning To protect potentially sensitive information in your connection string, move it to configuration (appsettings.json) and avoid hard-coding it here.
            optionsBuilder.UseSqlServer("Server=DESKTOP-HOQG9UU\\SQLEXPRESS;Database=SampleDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Categories table
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(e => e.CategoryId);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
        });

        // Products table and relationship to Category
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(e => e.ProductId);

            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");

            // Explicit FK to Category - prevents EF from guessing other relationships
            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Products_Categories_CategoryId");

            // If your model contains an accidental navigation to Proveedor, make sure EF does not create a shadow FK.
            // We do NOT configure any relationship to Proveedor here because the Products table doesn't have a ProveedorId column.
        });

        // Proveedores table
        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.ToTable("Proveedores");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();

            // The model has a navigation collection 'Productos' on Proveedor but the DB doesn't have a ProveedorId on Products.
            // Ignore that navigation so EF won't create a shadow FK 'ProveedorId' on Products.
            entity.Ignore(e => e.Productos);
        });

        // MovimientosInventario table and FK to Products
        modelBuilder.Entity<MovimientoInventario>(entity =>
        {
            entity.ToTable("MovimientosInventario");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.TipoMovimiento).HasMaxLength(20).IsRequired();
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(18,2)");

            entity.HasOne(m => m.Producto)
                .WithMany()
                .HasForeignKey(m => m.ProductoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_MovimientosInventario_Products_ProductoId");
        });

        // Ventas table
        modelBuilder.Entity<Venta>(entity =>
        {
            entity.ToTable("Ventas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ClienteNombre).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Total).HasColumnType("decimal(18,2)");
        });

        // DetalleVentas table and relationships to Venta and Products
        modelBuilder.Entity<DetalleVenta>(entity =>
        {
            entity.ToTable("DetalleVentas");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(18,2)");

            entity.HasOne(d => d.Venta)
                .WithMany(v => v.Detalles)
                .HasForeignKey(d => d.VentaId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DetalleVentas_Ventas_VentaId");

            entity.HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_DetalleVentas_Products_ProductoId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
