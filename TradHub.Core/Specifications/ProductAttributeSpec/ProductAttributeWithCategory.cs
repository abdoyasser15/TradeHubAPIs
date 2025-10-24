using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity;

namespace TradHub.Core.Specifications.ProductAttributeSpec
{
    public class ProductAttributeWithCategory : BaseSpecification<ProductAttribute>
    {
        public ProductAttributeWithCategory()
        {
            AddIncludes();
        }
        public ProductAttributeWithCategory(int id)
            :base(pa=>pa.Id==id)
        {
            AddIncludes();
        }
        private void AddIncludes()
        {
            Include.Add(pa => pa.CategoryAttribute);
        }
    }
}
