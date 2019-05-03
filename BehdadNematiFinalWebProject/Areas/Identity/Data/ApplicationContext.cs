using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BehdadNematiFinalWebProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BehdadNematiFinalWebProject.Models;

namespace BehdadNematiFinalWebProject.Models
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<image> images { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<PurchaseCart> PurchaseCarts { get; set; }
        public DbSet<PurchaseCart_Product> PurchaseCart_Products { get; set; }
        public DbSet<SpecialOffers> SpecialOffers { get; set; }
        public DbSet<TopProduct> TopProducts { get; set; }

    }
}
