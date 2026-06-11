using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace TradeHub.Repository.Data.Config.Basket
{
    public class BasketConfiguration : IEntityTypeConfiguration<TradHub.Core.Entity.Basket.Basket>
    {
        public void Configure(EntityTypeBuilder<TradHub.Core.Entity.Basket.Basket> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Items)
              .WithOne(x => x.Basket)
              .HasForeignKey(x => x.BasketId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.BuyerId)
                .IsRequired();
        }
    }
}
