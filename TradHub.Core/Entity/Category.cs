using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Entity
{
    public class Category : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
        public ICollection<CompanyCategory> CompanyCategories { get; set; } = new List<CompanyCategory>();
    }
}
