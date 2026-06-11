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
    public class FavouriteConfigration : IEntityTypeConfiguration<Favourite>
    {
        public void Configure(EntityTypeBuilder<Favourite> builder)
        {
            builder.ToTable("Favourites");

            builder.HasOne(f => f.User)
                   .WithMany(u => u.Favourites)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Product)
                     .WithMany(p => p.Favourites)
                     .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(f => new { f.UserId, f.ProductId })
                   .IsUnique();
        }
    }
}
