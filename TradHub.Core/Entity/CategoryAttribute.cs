using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Entity
{
    public class CategoryAttribute : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DataType { get; set; }
        public bool IsRequired { get; set; }
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}
