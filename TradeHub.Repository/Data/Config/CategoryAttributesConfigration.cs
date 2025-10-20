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
    public class CategoryAttributesConfigration : IEntityTypeConfiguration<CategoryAttribute>
    {
        public void Configure(EntityTypeBuilder<CategoryAttribute> builder)
        {
            builder.HasKey(ca => ca.Id);
            builder.Property(ca => ca.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(ca => ca.DataType)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.Property(ca => ca.IsRequired)
                   .IsRequired();
            builder.HasOne(ca => ca.Category)
                  .WithMany(c => c.CategoryAttributes)
                  .HasForeignKey(ca => ca.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
