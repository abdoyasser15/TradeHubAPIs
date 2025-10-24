using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity;

namespace TradHub.Core.Specifications.CategoryAttributeSpec
{
    public class CategoryAttributeWithCategorySpecification : BaseSpecification<CategoryAttribute>
    {
        public CategoryAttributeWithCategorySpecification()
        {
            AddIncludes();
        }
        private void AddIncludes()
        {
            Include.Add(C=> C.Category);
        }
    }
}
