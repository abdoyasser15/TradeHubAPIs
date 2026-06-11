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
    public class ProductRatingConfiguration : IEntityTypeConfiguration<ProductRating>
    {
        public void Configure(EntityTypeBuilder<ProductRating> builder)
        {
            builder.HasOne(r => r.Product)
            .WithMany(p => p.ProductRatings)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Property(r => r.RaitngValue)
                .IsRequired();

            builder.Property(r => r.Comment)
                .HasMaxLength(500);

            builder.HasIndex(r => new { r.ProductId, r.UserId })
                .IsUnique();
        }
    }
}
