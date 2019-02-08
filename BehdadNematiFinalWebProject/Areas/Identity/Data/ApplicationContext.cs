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
        public DbSet<brand> brands { get; set; }
        public DbSet<image> images { get; set; }
        public DbSet<product> products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<purchaseCart> purchaseCarts { get; set; }
        public DbSet<purchaseCart_product> purchaseCart_Products { get; set; }
        public DbSet<specialOffers> specialOffers { get; set; }
        public DbSet<topProduct> topProducts { get; set; }

    }
}
