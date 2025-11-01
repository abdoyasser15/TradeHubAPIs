using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public List<productattributedto> Attributes { get; set; } = new List<productattributedto>();
        public double AverageRating { get; set; }
        public int RatingCount { get; set; }
    }
}   
