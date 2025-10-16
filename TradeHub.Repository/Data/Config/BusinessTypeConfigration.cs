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
    public class BusinessTypeConfigration : IEntityTypeConfiguration<BusinessType>
    {
        public void Configure(EntityTypeBuilder<BusinessType> builder)
        {
            builder.HasKey(bt => bt.BusinessTypeId);

            builder.Property(bt => bt.Name).HasMaxLength(100).IsRequired();

            builder.Property(B=>B.IsActive).
                HasDefaultValue(true);

            
        }
    }
}
