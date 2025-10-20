using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Entity
{
    public class ProductAttributeConfigration : IEntityTypeConfiguration<ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductAttribute> builder)
        {
            builder.HasKey(pa => pa.Id);
            builder.Property(pa => pa.Value)
                   .IsRequired()
                   .HasMaxLength(500);
            builder.HasOne(pa => pa.Product)
                     .WithMany(p => p.ProductAttributes)
                     .HasForeignKey(pa => pa.ProductId)
                     .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
