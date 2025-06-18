using E_commerce_Admin_Dashboard.Models;
using Microsoft.EntityFrameworkCore;
using System;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Add User DbSet
    public DbSet<User> Users { get; set; }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<CustomerAddress> CustomerAddresses { get; set; }
    public DbSet<CustomerAddressDetail> CustomerAddressDetails { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<CustomerAction> CustomerActions { get; set; }
    public DbSet<View> Views { get; set; }
    public DbSet<Rate> Rates { get; set; }
    public DbSet<Refund> Refunds { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<AdminAction> AdminActions { get; set; }
    public DbSet<Warn> Warns { get; set; }
    public DbSet<Ban> Bans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- User to Admin / Customer one-to-one relationships ---
        modelBuilder.Entity<User>()
            .HasOne(u => u.AdminProfile)
            .WithOne(a => a.User)
            .HasForeignKey<Admin>(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasOne(u => u.CustomerProfile)
            .WithOne(c => c.User)
            .HasForeignKey<Customer>(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // --- Admin self-referencing (CreatedBy) ---
        modelBuilder.Entity<Admin>()
            .HasMany(a => a.AdminsCreated)
            .WithOne(a => a.CreatedByAdmin)
            .HasForeignKey(a => a.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // --- Unique constraints ---
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.PhoneNumber)
            .IsUnique();

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.SKU)
            .IsUnique();

        // --- Default values ---
        modelBuilder.Entity<Admin>()
            .Property(a => a.IsDeleted)
            .HasDefaultValue(false);

        modelBuilder.Entity<Admin>()
            .Property(a => a.IsSuperAdmin)
            .HasDefaultValue(false);

        modelBuilder.Entity<Customer>()
            .Property(c => c.IsDeleted)
            .HasDefaultValue(false);

        modelBuilder.Entity<User>()
            .Property(c => c.IsWarned)
            .HasDefaultValue(false);

        modelBuilder.Entity<User>()
            .Property(c => c.IsBanned)
            .HasDefaultValue(false);

        modelBuilder.Entity<User>()
            .Property(c => c.BannedDays)
            .HasDefaultValue(0);

        modelBuilder.Entity<User>()
            .Property(c => c.WarningLevel)
            .HasDefaultValue(0);

        modelBuilder.Entity<Customer>()
            .Property(c => c.LoyaltyPoints)
            .HasDefaultValue(0);

        modelBuilder.Entity<RefreshToken>()
            .Property(rt => rt.IsRevoked)
            .HasDefaultValue(false);

        modelBuilder.Entity<Product>()
            .Property(p => p.IsDeleted)
            .HasDefaultValue(false);

        modelBuilder.Entity<Product>()
            .Property(p => p.InStockQuantity)
            .HasDefaultValue(0);

        // --- Many-to-many join tables ---

        modelBuilder.Entity<CartItem>()
            .HasKey(ci => ci.Id);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Cart)
            .WithMany(c => c.CartItems)
            .HasForeignKey(ci => ci.CartId);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Product)
            .WithMany(p => p.CartItems)
            .HasForeignKey(ci => ci.ProductId);

        modelBuilder.Entity<PurchaseItem>()
            .HasKey(pi => pi.Id);

        modelBuilder.Entity<PurchaseItem>()
            .HasOne(pi => pi.Purchase)
            .WithMany(p => p.PurchaseItems)
            .HasForeignKey(pi => pi.PurchaseId);

        modelBuilder.Entity<PurchaseItem>()
            .HasOne(pi => pi.Product)
            .WithMany(p => p.PurchaseItems)
            .HasForeignKey(pi => pi.ProductId);

        // --- TPH for CustomerAction ---
        modelBuilder.Entity<CustomerAction>()
            .HasDiscriminator<CustomerActionType>("ActionType")
            .HasValue<View>(CustomerActionType.View)
            .HasValue<Rate>(CustomerActionType.Rate)
            .HasValue<Refund>(CustomerActionType.Refund);

        // --- TPH for AdminAction ---
        modelBuilder.Entity<AdminAction>()
            .HasDiscriminator<AdminActionType>("ActionType")
            .HasValue<Ban>(AdminActionType.Ban)
            .HasValue<Warn>(AdminActionType.Warn);

        // --- Seed initial super admin User + Admin ---

        var superAdminUserId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var superAdminId = new Guid("11111111-1111-1111-1111-111111111111");

        modelBuilder.Entity<User>().HasData(new User
        {
            Id = superAdminUserId,
            Email = "hponetaukyou@gmail.com",
            Password = "$2a$12$90UrUp1k5/Zmzx9b3Ms8YunIR5.zexGCRLj3G/ztUVzFUpQpFAC7.",
            Role = UserRole.Admin,
            CreatedAt = new DateTime(2025, 05, 21, 12, 00, 00, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2025, 05, 21, 12, 00, 00, DateTimeKind.Utc)
        });

        modelBuilder.Entity<Admin>().HasData(new Admin
        {
            Id = superAdminId,
            UserId = superAdminUserId,
            Name = "Hpone Tauk Nyi",
            PhoneNumber = "+959890079614",
            IsSuperAdmin = true,
            CreatedAt = new DateTime(2025, 05, 21, 12, 00, 00, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2025, 05, 21, 12, 00, 00, DateTimeKind.Utc),
            IsDeleted = false,
            CreatedBy = null
        });
    }
}
