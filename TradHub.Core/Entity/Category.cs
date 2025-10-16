using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Entity
{
    public class Category : BaseEntity
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<CompanyCategory> CompanyCategories { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
