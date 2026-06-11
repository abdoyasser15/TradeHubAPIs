using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Entity
{
    public class SubCategory : BaseEntity
    {
        public int SubCategoryId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<CategoryAttribute> CategoryAttributes { get; set; } = new List<CategoryAttribute>();
    }
}
