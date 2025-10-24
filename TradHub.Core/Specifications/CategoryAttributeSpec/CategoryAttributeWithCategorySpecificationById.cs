using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity;

namespace TradHub.Core.Specifications.CategoryAttributeSpec
{
    public class CategoryAttributeWithCategorySpecificationById : BaseSpecification<CategoryAttribute>
    {
        public CategoryAttributeWithCategorySpecificationById(int id) 
            : base(ca => ca.Id == id)
        {
            AddIncludes();
        }
        private void AddIncludes()
        {
            Include.Add(C => C.Category);
        }
    }
}
