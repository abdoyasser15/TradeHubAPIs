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
    public class CompanyRatingsConfiguration : IEntityTypeConfiguration<CompanyRatings>
    {
        public void Configure(EntityTypeBuilder<CompanyRatings> builder)
        {
            builder.HasOne(r => r.Company)
            .WithMany(c => c.CompanyRatings)
            .HasForeignKey(r => r.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Property(r => r.RatingValue)
                .IsRequired();

            builder.Property(r => r.Comment)
                .HasMaxLength(500);

            builder.HasIndex(r => new { r.CompanyId, r.UserId })
                .IsUnique();
        }
    }
}
