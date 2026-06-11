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
    public class ProductConfigration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(p => p.Company)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CompanyId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.SubCategory)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.SubCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.Description)
                 .IsRequired(false);

            builder.Property(p => p.ImageUrl)
                .IsRequired(false);
        }
    }
}
