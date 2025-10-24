using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity.Identity;

namespace TradeHub.Repository.Data.Config
{
    public class AppUserConfigration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(a=>a.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(a=>a.LastName).HasMaxLength(50).IsRequired();
            builder.Property(a=>a.Email).HasMaxLength(100).IsRequired();
            builder.Property(a=>a.PhoneNumber).HasMaxLength(11);
            builder.Property(a=>a.PasswordHash).IsRequired();
            
            builder.Property(a=>a.AccountType)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(e => e.Role)
            .HasConversion<int>().IsRequired();

            builder.Property(u => u.CreatedAt)
             .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(u => u.Email).IsUnique();

            builder.HasOne(u => u.Company)
                .WithMany(c=>c.Staff)
                .HasForeignKey(u => u.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
