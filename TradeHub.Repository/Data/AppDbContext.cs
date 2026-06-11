using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity;
using TradHub.Core.Entity.Basket;
using TradHub.Core.Entity.Identity;
using TradHub.Core.Entity.Orders;
using TradHub.Core.Entity.Payments;
using TradHub.Core.Enums;

namespace TradeHub.Repository
{
    public class AppDbContext : IdentityDbContext<AppUser , IdentityRole , string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options){}
        public DbSet<BusinessType> BusinessTypes { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyCategory> CompanyCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OtpCode> otpCodes { get; set; }
        public DbSet<CategoryAttribute> CategoryAttributes { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<ProductRating> productRatings { get; set; }
        public DbSet<CompanyRatings> CompanyRatings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ProductOptions> ProductOptions { get; set; }   
        public DbSet<ProductOptionValue> ProductOptionValues { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            builder.Entity<SubCategory>()
            .HasIndex(c => c.Name)
            .IsUnique();
        }
    }
}
