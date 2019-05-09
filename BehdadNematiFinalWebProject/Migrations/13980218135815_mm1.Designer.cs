﻿// <auto-generated />
using System;
using BehdadNematiFinalWebProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BehdadNematiFinalWebProject.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("13980218135815_mm1")]
    partial class mm1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BehdadNematiFinalWebProject.Areas.Identity.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("BehdadNematiFinalWebProject.Models.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("name");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("BehdadNematiFinalWebProject.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Brand_Id");

                    b.Property<string>("EnglishName");

                    b.Property<int>("ProductType_Id");

                    b.Property<int>("count");

                    b.Property<bool>("isAproved");

                    b.Property<int>("price");

                    b.HasKey("Id");

                    b.HasIndex("Brand_Id");

                    b.HasIndex("ProductType_Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("BehdadNematiFinalWebProject.Models.ProductType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("name");

                    b.HasKey("Id");

                    b.ToTable("ProductTypes");
                });

            modelBuilder.Entity("BehdadNematiFinalWebProject.Models.PurchaseCart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("exDate");

                    b.Property<bool>("isPaid");

                    b.Property<DateTime>("pDate");

                    b.Property<string>("user_Id");

                    b.HasKey("Id");

                    b.HasIndex("user_Id");

                    b.ToTable("PurchaseCarts");
                });

            modelBuilder.Entity("BehdadNematiFinalWebProject.Models.PurchaseCart_Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Product_Id");

                    b.Property<int>("PurchaseCart_Id");

                    b.Property<int>("count");

                    b.HasKey("Id");

                    b.HasIndex("Product_Id");

                    b.HasIndex("PurchaseCart_Id");

                    b.ToTable("PurchaseCart_Products");
                });

            modelBuilder.Entity("BehdadNematiFinalWebProject.Models.SpecialOffers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Product_Id");

                    b.HasKey("Id");

                    b.HasIndex("Product_Id");

                    b.ToTable("SpecialOffers");
                });

            modelBuilder.Entity("BehdadNematiFinalWebProject.Models.TopProducts", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Product_Id");

                    b.HasKey("Id");

                    b.HasIndex("Product_Id");

                    b.ToTable("TopProducts");
                });

            modelBuilder.Entity("BehdadNematiFinalWebProject.Models.image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Product_Id");

                    b.Property<byte[]>("img");

                    b.HasKey("Id");

                    b.HasIndex("Product_Id");

                    b.ToTable("images");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("BehdadNematiFinalWebProject.Models.Product", b =>
                {
                    b.HasOne("BehdadNematiFinalWebProject.Models.Brand", "Brand")
                        .WithMany("Products")
                        .HasForeignKey("Brand_Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BehdadNematiFinalWebProject.Models.ProductType", "ProductType")
                        .WithMany("Products")
                        .HasForeignKey("ProductType_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BehdadNematiFinalWebProject.Models.PurchaseCart", b =>
                {
                    b.HasOne("BehdadNematiFinalWebProject.Areas.Identity.Data.ApplicationUser", "user")
                        .WithMany()
                        .HasForeignKey("user_Id");
                });

            modelBuilder.Entity("BehdadNematiFinalWebProject.Models.PurchaseCart_Product", b =>
                {
                    b.HasOne("BehdadNematiFinalWebProject.Models.Product", "Product")
                        .WithMany("PurchaseCart_Products")
                        .HasForeignKey("Product_Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BehdadNematiFinalWebProject.Models.PurchaseCart", "PurchaseCart")
                        .WithMany()
                        .HasForeignKey("PurchaseCart_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BehdadNematiFinalWebProject.Models.SpecialOffers", b =>
                {
                    b.HasOne("BehdadNematiFinalWebProject.Models.Product", "Product")
                        .WithMany("SpecialOffers")
                        .HasForeignKey("Product_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BehdadNematiFinalWebProject.Models.TopProducts", b =>
                {
                    b.HasOne("BehdadNematiFinalWebProject.Models.Product", "Product")
                        .WithMany("TopProducts")
                        .HasForeignKey("Product_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BehdadNematiFinalWebProject.Models.image", b =>
                {
                    b.HasOne("BehdadNematiFinalWebProject.Models.Product", "Product")
                        .WithMany("images")
                        .HasForeignKey("Product_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BehdadNematiFinalWebProject.Areas.Identity.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BehdadNematiFinalWebProject.Areas.Identity.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BehdadNematiFinalWebProject.Areas.Identity.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BehdadNematiFinalWebProject.Areas.Identity.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
