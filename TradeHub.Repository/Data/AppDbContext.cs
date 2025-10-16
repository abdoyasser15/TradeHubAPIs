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
using TradHub.Core.Entity.Identity;
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
        public DbSet<Location> Locations { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OtpCode> otpCodes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);            
        }
    }
}
