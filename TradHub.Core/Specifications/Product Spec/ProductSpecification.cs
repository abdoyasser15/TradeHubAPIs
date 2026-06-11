using StackExchange.Redis;
using TradHub.Core.Entity;
using TradHub.Core.Specifications;
using TradHub.Core.Specifications.Product_Spec;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecParams spec)
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

        if (!string.IsNullOrEmpty(spec.Sort))
        {
            switch (spec.Sort)
            {
                case "priceasc":
                    AddOrderBy(p => p.Price);
                    break;

                case "pricedesc":
                    AddOrderByDescending(p => p.Price);
                    break;

                case "raitingdesc":
                    AddOrderByDescending(p =>
                        p.ProductRatings.Any()
                            ? p.ProductRatings.Average(r => r.RaitngValue)
                            : 0);
                    break;

                default:
                    AddOrderBy(p => p.Name);
                    break;
            }
        }
        else
        {
            AddOrderBy(p => p.Name);
        }

        ApplyPaging(spec.PageSize * (spec.pageIndex - 1), spec.PageSize);
        ApplyNoTracking();
    }

    public ProductSpecification(int productId)
        : base(p => p.Id == productId)
    {
        AddIncludes();
    }

    private void AddIncludes()
    {
        Include.Add(p => p.SubCategory);
        Include.Add(p => p.Company);
        Include.Add(p => p.ProductAttributes);
        Include.Add(p => p.ProductRatings);
        Include.Add(p => p.Favourites);

        IncludeStrings.Add($"{nameof(Product.ProductAttributes)}.{nameof(ProductAttribute.CategoryAttribute)}");
    }
}