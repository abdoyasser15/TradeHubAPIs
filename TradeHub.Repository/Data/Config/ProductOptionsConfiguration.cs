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
    public class ProductOptionsConfiguration : IEntityTypeConfiguration<ProductOptions>
    {
        public void Configure(EntityTypeBuilder<ProductOptions> builder)
        {
            builder.HasKey(po => po.ProductOptionId);

            builder.Property(po => po.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(po => po.IsRequired)
                   .IsRequired();

            builder.Property(po => po.AllowMultiple);

            builder.HasOne(po => po.Product)
                   .WithMany(p => p.ProductOptions)
                   .HasForeignKey(po => po.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p=> p.ProductOptionValues)
                   .WithOne(pov => pov.ProductOptions)
                   .HasForeignKey(pov => pov.ProductOptionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
