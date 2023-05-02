using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WatchShop.DAL.DbModels;

public partial class DefaultDbContext : DbContext
{
    public DefaultDbContext()
    {
        Database.EnsureCreated();
    }

    public DefaultDbContext(DbContextOptions<DefaultDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemsInCart> ItemsInCarts { get; set; }

    public virtual DbSet<OneTimePassword> OneTimePasswords { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
            var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};Connect Timeout=30;TrustServerCertificate=True;User ID=sa;Password={dbPassword}";
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseSqlServer(connectionString);
            //optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=WatchStoreDB;Integrated security=True;TrustServerCertificate=True");
            //optionsBuilder.UseSqlServer("Data Source=mssql,1433;Initial Catalog=WatchStoreDB;Integrated security=False;TrustServerCertificate=True; User Id = sa; Password=Pavel0503!@#");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Carts__3214EC27EE557846");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Carts.UserID");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Items__3214EC27A34A3B78");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Picture).HasMaxLength(1000);
            entity.Property(e => e.Rating).HasColumnType("decimal(2, 1)");
        });

        modelBuilder.Entity<ItemsInCart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ItemsInC__3214EC27B4367A41");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");

            entity.HasOne(d => d.Cart).WithMany(p => p.ItemsInCarts)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("FK_ItemsInCarts.CartID");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemsInCarts)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_ItemsInCarts.ItemID");
        });

        modelBuilder.Entity<OneTimePassword>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OneTimeP__3214EC27D475D0E4");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Password).HasMaxLength(16);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.OneTimePasswords)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Passwords.UserID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC2774B8E652");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Login).HasMaxLength(25);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
