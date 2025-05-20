using E_commerce_Admin_Dashboard.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Admin> Admins { get; set; }
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

        // Self-referencing in Admin table
        modelBuilder.Entity<Admin>()
            .HasMany(a => a.AdminsCreated)
            .WithOne(a => a.CreatedByAdmin)
            .HasForeignKey(a => a.CreatedBy);

        // To avoid cascading deletion in admin table 
        modelBuilder.Entity<Admin>()
            .HasMany(a => a.AdminsCreated)
            .WithOne(a => a.CreatedByAdmin)
            .HasForeignKey(a => a.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);


        // Unique constraints
        modelBuilder.Entity<Admin>()
            .HasIndex(a => a.Email)
            .IsUnique(true);

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Email)
            .IsUnique(true);

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.PhoneNumber)
            .IsUnique(true);

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.SKU)
            .IsUnique(true);

        // Default values
        modelBuilder.Entity<Admin>()
            .Property(a => a.IsDeleted)
            .HasDefaultValue(false);

        modelBuilder.Entity<Admin>()
            .Property(a => a.IsSuperAdmin)
            .HasDefaultValue(false);

        modelBuilder.Entity<Customer>()
            .Property(c => c.IsDeleted)
            .HasDefaultValue(false);

        modelBuilder.Entity<Customer>()
            .Property(c => c.IsWarned)
            .HasDefaultValue(false);

        modelBuilder.Entity<Customer>()
            .Property (c => c.IsBanned)
            .HasDefaultValue(false);

        modelBuilder.Entity<Customer>()
            .Property(c => c.BannedDays)
            .HasDefaultValue(0);

        modelBuilder.Entity<Customer>()
            .Property(c => c.WarningLevel)
            .HasDefaultValue(0);

        modelBuilder.Entity<Customer>()
            .Property(c => c.LoyaltyPoints)
            .HasDefaultValue(0);

        modelBuilder.Entity<Product>()
            .Property(p => p.IsDeleted)
            .HasDefaultValue(false);

        modelBuilder.Entity<Product>()
            .Property(p => p.InStockQuantity)
            .HasDefaultValue(0);



        // Many-to-may join tables
        // Cart and product
        modelBuilder.Entity<CartItem>()
            .HasKey(ci => ci.Id);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Cart)
            .WithMany(a => a.CartItems)
            .HasForeignKey(ci => ci.CartId);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Product)
            .WithMany(p => p.CartItems)
            .HasForeignKey(ci => ci.ProductId);

        // Purchase and product
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

        // CustomerAction TPH Configuration
        modelBuilder.Entity<CustomerAction>()
            .HasDiscriminator<CustomerActionType>("ActionType")
            .HasValue<View>(CustomerActionType.View)
            .HasValue<Rate>(CustomerActionType.Rate)
            .HasValue<Refund>(CustomerActionType.Refund);

        // AdminAction TPH Configuration
        modelBuilder.Entity<AdminAction>()
            .HasDiscriminator<AdminActionType>("ActionType")
            .HasValue<Ban>(AdminActionType.Ban)
            .HasValue<Warn>(AdminActionType.Warn);

    }
}
