using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity;

namespace TradHub.Core.Specifications.Product_Spec
{
    public class RandomProductSpecification : BaseSpecification<Product>
    {
        public RandomProductSpecification(ProductSpecParams spec)
            : base(p =>
                (string.IsNullOrEmpty(spec.Search) ||
                 ((p.Name != null && p.Name.ToLower().Contains(spec.Search.ToLower())) ||
                  (p.Description != null && p.Description.ToLower().Contains(spec.Search.ToLower()))))
                &&
                (!spec.categoryId.HasValue || p.SubCategoryId == spec.categoryId)
                &&
                (!spec.CompanyId.HasValue || p.CompanyId == spec.CompanyId)
            )
        {
            AddIncludes();

            AddOrderBy(p => Guid.NewGuid());


            ApplyPaging(spec.PageSize * (spec.pageIndex - 1), spec.PageSize);
            ApplyNoTracking();
        }

        private void AddIncludes()
        {
            Include.Add(p => p.SubCategory);
            Include.Add(p => p.Company);
            Include.Add(p => p.ProductAttributes);
            Include.Add(p => p.ProductRatings);
            Include.Add(p => p.Favourites);

            IncludeStrings.Add($"{nameof(Product.SubCategory)}.{nameof(SubCategory.Category)}");

            IncludeStrings.Add($"{nameof(Product.ProductAttributes)}.{nameof(ProductAttribute.CategoryAttribute)}");
        }
    }
}
