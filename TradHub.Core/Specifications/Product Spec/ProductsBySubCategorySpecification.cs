using TradHub.Core.Entity;

namespace TradHub.Core.Specifications.Product_Spec
{
    public class ProductsBySubCategorySpecification : BaseSpecification<Product>
    {
        public ProductsBySubCategorySpecification(int subCategoryId)
            : base(p => p.SubCategoryId == subCategoryId)
        {
            AddIncludes();
            ApplyNoTracking();
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
}