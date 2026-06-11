using StackExchange.Redis;
using TradHub.Core.Entity;
using TradHub.Core.Specifications;
using TradHub.Core.Specifications.Favourite_Spec;

public class FavouriteSpecifiaction : BaseSpecification<Favourite>
{
    public FavouriteSpecifiaction(FavouriteSpecParams specParams)
        : base(f =>
            (string.IsNullOrEmpty(specParams.Search) ||
             (f.Product != null &&
              f.Product.Name != null &&
              f.Product.Name.ToLower().Contains(specParams.Search.ToLower())))
            &&
            (string.IsNullOrEmpty(specParams.UserId) ||
             f.UserId == specParams.UserId)
            &&
            (!specParams.ProductId.HasValue ||
             f.ProductId == specParams.ProductId)
        )
    {
        AddInclude();

        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );

        ApplyNoTracking();
    }

    public FavouriteSpecifiaction(string userId)
        : base(f => f.UserId == userId)
    {
        AddInclude();
        ApplyNoTracking();
    }

    private void AddInclude()
    {
        Include.Add(f => f.Product);
        Include.Add(f => f.User);

        IncludeStrings.Add("Product.Company");
        IncludeStrings.Add("Product.SubCategory");
        IncludeStrings.Add("Product.SubCategory.Category");
        IncludeStrings.Add("Product.ProductAttributes");
        IncludeStrings.Add("Product.ProductAttributes.CategoryAttribute");
        IncludeStrings.Add("Product.ProductRatings");
    }
}