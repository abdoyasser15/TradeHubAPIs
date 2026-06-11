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
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProductName).IsRequired().HasMaxLength(250);
            builder.Property(x => x.ImageUrl).HasMaxLength(500);
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            builder.Property(x => x.CompanyId).IsRequired();
        }
    }
}
