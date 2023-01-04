using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace online_store.Models;

public partial class OnlineStoreContext : DbContext
{
    public OnlineStoreContext()
    {
    }

    public OnlineStoreContext(DbContextOptions<OnlineStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=online store;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Address");

            entity.Property(e => e.AddressLine)
                .HasMaxLength(200)
                .HasColumnName("address_line");
            entity.Property(e => e.City)
                .HasMaxLength(150)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(150)
                .HasColumnName("country");
            entity.Property(e => e.HomeNumber).HasColumnName("Home_Number");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(50)
                .HasColumnName("postal_code");
            entity.Property(e => e.StreetNumber)
                .HasMaxLength(50)
                .HasColumnName("Street_number");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Cart_Product");

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Cart_User");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(150);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country");

            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CountryName)
                .HasMaxLength(150)
                .HasColumnName("country_name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.AdderssId).HasColumnName("Adderss_ID");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("date")
                .HasColumnName("created_date");
            entity.Property(e => e.DeliveryCost)
                .HasColumnType("numeric(18, 5)")
                .HasColumnName("delivery_cost");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(50)
                .HasColumnName("Order_Status");
            entity.Property(e => e.PaymentMethod).HasMaxLength(150);
            entity.Property(e => e.ShippedDate)
                .HasColumnType("date")
                .HasColumnName("shipped_date");
            entity.Property(e => e.TotatlPrice).HasColumnType("numeric(18, 5)");
            entity.Property(e => e.TotatlPriceForProducts)
                .HasColumnType("numeric(18, 5)")
                .HasColumnName("TotatlPrice_for_products");

            entity.HasOne(d => d.Adderss).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AdderssId)
                .HasConstraintName("FK_Order_Address");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Order_User");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.ToTable("OrderProduct");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Price)
                .HasColumnType("numeric(18, 5)")
                .HasColumnName("price");
            entity.Property(e => e.PricePerItem)
                .HasColumnType("numeric(18, 5)")
                .HasColumnName("price_per_item");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderProduct_Order");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Price).HasColumnType("numeric(18, 5)");
            entity.Property(e => e.ProductName).HasMaxLength(150);
            entity.Property(e => e.VendorId).HasColumnName("VendorID");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Product_Category");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Products)
                .HasForeignKey(d => d.VendorId)
                .HasConstraintName("FK_Product_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(200);

            entity.HasOne(d => d.Address).WithMany(p => p.Users)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK_User_Address");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
