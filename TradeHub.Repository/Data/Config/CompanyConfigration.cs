using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity;

namespace TradeHub.Repository.Data.Config
{
    public class CompanyConfigration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(c => c.CompanyId);
            builder.Property(c => c.CompanyId).HasDefaultValueSql("NEWID()");

            builder.Property(c=>c.BusinessName).HasMaxLength(100).IsRequired();

            builder.HasIndex(x=>x.BusinessName).IsUnique();

            builder.Property(c => c.TaxNumber).HasMaxLength(50);
            builder.Property(c => c.LogoUrl).HasMaxLength(300);

            builder.HasOne(C=>C.BusinessType)
                .WithMany(bt=>bt.Companies)
                .HasForeignKey(c=>c.BusinessTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.CreatedBy)
                .WithMany()
                .HasForeignKey(c => c.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Location)
                .WithMany(L=>L.Companies)
                .HasForeignKey(c => c.LocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
