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
    public class OrderItemOptionConfiguration : IEntityTypeConfiguration<OrderItemOption>
    {
        public void Configure(EntityTypeBuilder<OrderItemOption> builder)
        {
            builder.HasKey(oio => oio.Id);

            builder.Property(oio => oio.OptionName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(oio => oio.ValueName)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.HasOne(oio => oio.OrderItem)
                .WithMany(oi => oi.Options)
                .HasForeignKey(oio => oio.OrderItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
