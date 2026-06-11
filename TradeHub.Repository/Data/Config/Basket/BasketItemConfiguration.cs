using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity.Basket;

namespace TradeHub.Repository.Data.Config.Basket
{
    public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProductName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.LogoUrl)
                   .HasMaxLength(500);

            builder.Property(x => x.Price)
                   .HasColumnType("decimal(18,2)");
        }
    }
}
