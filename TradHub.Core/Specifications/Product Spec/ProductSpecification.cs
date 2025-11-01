using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity;

namespace TradHub.Core.Specifications.Product_Spec
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(ProductSpecParams Spec)
            : base(P =>
            (string.IsNullOrEmpty(Spec.Search) || (P.Name.ToLower().Contains(Spec.Search) || P.Description!.ToLower().Contains(Spec.Search)))
            &&
            (!Spec.categoryId.HasValue || P.CategoryId == Spec.categoryId) &&
            (!Spec.CompanyId.HasValue || P.CompanyId == Spec.CompanyId)
        )
        {
            AddIncludes();
            if (!string.IsNullOrEmpty(Spec.Sort))
            {
                switch (Spec.Sort)
                {
                    case "priceasc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    case "raitingdesc":
                        AddOrderByDescending(p =>
                            p.ProductRaitings.Any()
                                ? p.ProductRaitings.Average(r => r.RaitingValue)
                                : 0);
                        break;
                    default:
                        AddOrderBy(P=>P.Name);
                        break;
                }
            }
            ApplyPaging(Spec.PageSize * (Spec.pageIndex - 1), Spec.PageSize);
            ApplyNoTracking();
        }
        public ProductSpecification(int productId)
        {
            AddIncludes();
        }
        private void AddIncludes()
        {
            Include.Add(p => p.Category);
            Include.Add(p => p.Company);
            Include.Add(p => p.ProductAttributes);
            ThenInclude();
            Include.Add(P=>P.ProductRaitings);
        }
        private void ThenInclude()
        {
            IncludeStrings.Add($"{nameof(Product.ProductAttributes)}.{nameof(ProductAttribute.CategoryAttribute)}");
        }
    }
}
