using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Entity
{
    public class Product : BaseEntity
    {
        public int Id { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<ProductAttribute> ProductAttributes { get; set; } = new List<ProductAttribute>();
        public ICollection<ProductRating> ProductRatings { get; set; } = new List<ProductRating>();
        public ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();
        public ICollection<ProductOptions> ProductOptions { get; set; } = new List<ProductOptions>();

    }
}
