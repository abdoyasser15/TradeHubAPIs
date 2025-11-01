using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity;

namespace TradHub.Core.Specifications.Product_Spec
{
    public class ProductCountSpecifications : BaseSpecification<Product>
    {
        public ProductCountSpecifications(ProductSpecParams Spec)
            : base(P =>
            (string.IsNullOrEmpty(Spec.Search) || (P.Name.ToLower().Contains(Spec.Search) || P.Description!.ToLower().Contains(Spec.Search)))
            &&
            (!Spec.categoryId.HasValue || P.CategoryId == Spec.categoryId) &&
            (!Spec.CompanyId.HasValue || P.CompanyId == Spec.CompanyId)
        )
        {
            
        }
    }
}
