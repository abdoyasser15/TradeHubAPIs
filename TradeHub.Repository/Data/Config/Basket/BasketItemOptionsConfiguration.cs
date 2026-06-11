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
    public class BasketItemOptionsConfiguration : IEntityTypeConfiguration<BasketItemOptions>
    {
        public void Configure(EntityTypeBuilder<BasketItemOptions> builder)
        {
                builder.HasKey(bio => bio.BaskItemOptionId);

            builder.Property(bio => bio.OptionName)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(bio => bio.ValueName)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.HasOne(bio => bio.BasketItem)
                    .WithMany(bi => bi.Options)
                    .HasForeignKey(bio => bio.BasketItemId)
                    .OnDelete(DeleteBehavior.Cascade);
    
                builder.HasOne(bio => bio.ProductOptionValue)
                    .WithMany()
                    .HasForeignKey(bio => bio.ProductOptionValueId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
