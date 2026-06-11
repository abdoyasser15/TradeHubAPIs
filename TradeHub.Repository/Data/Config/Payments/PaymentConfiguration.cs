using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity.Payments;

namespace TradeHub.Repository.Data.Config.Payments
{
    public class PaymentConfiguration : IEntityTypeConfiguration<PaymentTransaction>
    {
        public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Provider).IsRequired().HasMaxLength(50);
            builder.Property(x => x.RawPayload).IsRequired();
        }
    }
}
