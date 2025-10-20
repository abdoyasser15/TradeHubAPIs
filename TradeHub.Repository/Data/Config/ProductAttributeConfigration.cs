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
    public class ProductAttributeConfigration : IEntityTypeConfiguration<ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductAttribute> builder)
        {
            builder.HasOne(pa => pa.Product)
                   .WithMany(p => p.ProductAttributes)
                   .HasForeignKey(pa => pa.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);  
        }
    }
}
