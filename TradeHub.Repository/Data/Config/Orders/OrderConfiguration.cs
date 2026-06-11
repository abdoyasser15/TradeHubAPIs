using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity.Orders;

namespace TradeHub.Repository.Data.Config.Orders
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.BuyerId).IsRequired();
            builder.Property(x => x.SubTotal).HasColumnType("decimal(18,2)");
            builder.Property(x => x.DeliveryFee).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Total).HasColumnType("decimal(18,2)");

            builder.HasMany(x => x.Items)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
