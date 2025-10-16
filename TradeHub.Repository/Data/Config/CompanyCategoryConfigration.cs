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
    public class CompanyCategoryConfigration : IEntityTypeConfiguration<CompanyCategory>
    {
        public void Configure(EntityTypeBuilder<CompanyCategory> builder)
        {
            builder.HasKey(cc => new { cc.CompanyId, cc.CategoryId });
            builder
            .Property(c => c.CompanyId)
            .ValueGeneratedNever();
            builder.HasOne(cc => cc.Company)
                .WithMany(c => c.CompanyCategories)
                .HasForeignKey(cc => cc.CompanyId);

            builder.HasOne(cc => cc.Category)
                .WithMany(c => c.CompanyCategories)
                .HasForeignKey(cc => cc.CategoryId);

        }
    }
}
