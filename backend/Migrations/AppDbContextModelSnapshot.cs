﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace E_commerce_Admin_Dashboard.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Admin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsSuperAdmin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Admins");

                    b.HasData(
                        new
                        {
                            Id = new Guid("11111111-1111-1111-1111-111111111111"),
                            IsDeleted = false,
                            IsSuperAdmin = true,
                            Name = "Hpone Tauk Nyi",
                            UserId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
                        });
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.AdminAction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ActionType")
                        .HasColumnType("int");

                    b.Property<Guid>("AdminId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TargetCustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("TargetCustomerId");

                    b.ToTable("AdminActions");

                    b.HasDiscriminator<int>("ActionType");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.CartItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CartId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.HasIndex("ProductId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("LoyaltyPoints")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.CustomerAction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ActionType")
                        .HasColumnType("int");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ProductId");

                    b.ToTable("CustomerActions");

                    b.HasDiscriminator<int>("ActionType");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.CustomerAddress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("CustomerAddresses");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.CustomerAddressDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CustomerAddressId");

                    b.HasIndex("CustomerId");

                    b.ToTable("CustomerAddressDetails");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Favorite", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("bit");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ProductId");

                    b.ToTable("Favorites");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("InStockQuantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<decimal>("PriceInMMK")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Rating")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SKU")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("SKU")
                        .IsUnique();

                    b.ToTable("Products");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Purchase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PaymentMethod")
                        .HasColumnType("int");

                    b.Property<int>("PaymentOption")
                        .HasColumnType("int");

                    b.Property<Guid>("ShippingAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerAddressId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.PurchaseItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PurchaseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("PurchaseId");

                    b.ToTable("PurchaseItems");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRevoked")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("BannedDays")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsBanned")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsWarned")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("WarningLevel")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                            CreatedAt = new DateTime(2025, 5, 21, 12, 0, 0, 0, DateTimeKind.Utc),
                            Email = "hponetaukyou@gmail.com",
                            IsBanned = false,
                            IsDeleted = false,
                            IsWarned = false,
                            Password = "$2a$12$90UrUp1k5/Zmzx9b3Ms8YunIR5.zexGCRLj3G/ztUVzFUpQpFAC7.",
                            PhoneNumber = "+959890079614",
                            Role = 0,
                            UpdatedAt = new DateTime(2025, 5, 21, 12, 0, 0, 0, DateTimeKind.Utc)
                        });
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Ban", b =>
                {
                    b.HasBaseType("E_commerce_Admin_Dashboard.Models.AdminAction");

                    b.Property<int>("DurationDays")
                        .HasColumnType("int");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Warn", b =>
                {
                    b.HasBaseType("E_commerce_Admin_Dashboard.Models.AdminAction");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Severity")
                        .HasColumnType("int");

                    b.ToTable("AdminActions", t =>
                        {
                            t.Property("Reason")
                                .HasColumnName("Warn_Reason");
                        });

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Rate", b =>
                {
                    b.HasBaseType("E_commerce_Admin_Dashboard.Models.CustomerAction");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Refund", b =>
                {
                    b.HasBaseType("E_commerce_Admin_Dashboard.Models.CustomerAction");

                    b.Property<Guid>("PurchaseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.View", b =>
                {
                    b.HasBaseType("E_commerce_Admin_Dashboard.Models.CustomerAction");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Admin", b =>
                {
                    b.HasOne("E_commerce_Admin_Dashboard.Models.Admin", "CreatedByAdmin")
                        .WithMany("AdminsCreated")
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("E_commerce_Admin_Dashboard.Models.User", "User")
                        .WithOne("AdminProfile")
                        .HasForeignKey("E_commerce_Admin_Dashboard.Models.Admin", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedByAdmin");

                    b.Navigation("User");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.AdminAction", b =>
                {
                    b.HasOne("E_commerce_Admin_Dashboard.Models.Admin", "Admin")
                        .WithMany("AdminActions")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("E_commerce_Admin_Dashboard.Models.Customer", null)
                        .WithMany("AdminActions")
                        .HasForeignKey("CustomerId");

                    b.HasOne("E_commerce_Admin_Dashboard.Models.User", "TargetCustomer")
                        .WithMany()
                        .HasForeignKey("TargetCustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("TargetCustomer");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Cart", b =>
                {
                    b.HasOne("E_commerce_Admin_Dashboard.Models.Customer", null)
                        .WithMany("Carts")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.CartItem", b =>
                {
                    b.HasOne("E_commerce_Admin_Dashboard.Models.Cart", "Cart")
                        .WithMany("CartItems")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("E_commerce_Admin_Dashboard.Models.Product", "Product")
                        .WithMany("CartItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Customer", b =>
                {
                    b.HasOne("E_commerce_Admin_Dashboard.Models.User", "User")
                        .WithOne("CustomerProfile")
                        .HasForeignKey("E_commerce_Admin_Dashboard.Models.Customer", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.CustomerAction", b =>
                {
                    b.HasOne("E_commerce_Admin_Dashboard.Models.Customer", "Customer")
                        .WithMany("CustomerActions")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("E_commerce_Admin_Dashboard.Models.Product", "Product")
                        .WithMany("CustomerActions")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.CustomerAddressDetail", b =>
                {
                    b.HasOne("E_commerce_Admin_Dashboard.Models.CustomerAddress", "CustomerAddress")
                        .WithMany("CustomerAddressDetails")
                        .HasForeignKey("CustomerAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("E_commerce_Admin_Dashboard.Models.Customer", "Customer")
                        .WithMany("CustomerAddresseDetails")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("CustomerAddress");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Favorite", b =>
                {
                    b.HasOne("E_commerce_Admin_Dashboard.Models.Customer", "Customer")
                        .WithMany("Favorites")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("E_commerce_Admin_Dashboard.Models.Product", "Product")
                        .WithMany("Favorites")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Purchase", b =>
                {
                    b.HasOne("E_commerce_Admin_Dashboard.Models.CustomerAddress", "CustomerAddress")
                        .WithMany("Purchases")
                        .HasForeignKey("CustomerAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("E_commerce_Admin_Dashboard.Models.Customer", "Customer")
                        .WithMany("Purchases")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("CustomerAddress");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.PurchaseItem", b =>
                {
                    b.HasOne("E_commerce_Admin_Dashboard.Models.Product", "Product")
                        .WithMany("PurchaseItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("E_commerce_Admin_Dashboard.Models.Purchase", "Purchase")
                        .WithMany("PurchaseItems")
                        .HasForeignKey("PurchaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Purchase");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.RefreshToken", b =>
                {
                    b.HasOne("E_commerce_Admin_Dashboard.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Admin", b =>
                {
                    b.Navigation("AdminActions");

                    b.Navigation("AdminsCreated");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Cart", b =>
                {
                    b.Navigation("CartItems");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Customer", b =>
                {
                    b.Navigation("AdminActions");

                    b.Navigation("Carts");

                    b.Navigation("CustomerActions");

                    b.Navigation("CustomerAddresseDetails");

                    b.Navigation("Favorites");

                    b.Navigation("Purchases");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.CustomerAddress", b =>
                {
                    b.Navigation("CustomerAddressDetails");

                    b.Navigation("Purchases");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Product", b =>
                {
                    b.Navigation("CartItems");

                    b.Navigation("CustomerActions");

                    b.Navigation("Favorites");

                    b.Navigation("PurchaseItems");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.Purchase", b =>
                {
                    b.Navigation("PurchaseItems");
                });

            modelBuilder.Entity("E_commerce_Admin_Dashboard.Models.User", b =>
                {
                    b.Navigation("AdminProfile")
                        .IsRequired();

                    b.Navigation("CustomerProfile")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
