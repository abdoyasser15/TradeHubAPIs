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
    public class ProductOptionValuesConfiguration : IEntityTypeConfiguration<ProductOptionValue>
    {
        public void Configure(EntityTypeBuilder<ProductOptionValue> builder)
        {
            builder.HasKey(pov => pov.ProductOptionValueId);

            builder.Property(pov => pov.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(pov => pov.ExtraPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.HasOne(pov => pov.ProductOptions)
                   .WithMany(po => po.ProductOptionValues)
                   .HasForeignKey(pov => pov.ProductOptionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
