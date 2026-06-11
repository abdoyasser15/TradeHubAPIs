using StackExchange.Redis;
using TradHub.Core.Entity.Orders;
using TradHub.Core.Specifications;
using TradHub.Core.Specifications.OrderSpec;

public class OrderSpecification : BaseSpecification<TradHub.Core.Entity.Orders.Order>
{
    public OrderSpecification(string buyerId, OrderSpecParams spec)
        : base(o => o.BuyerId == buyerId)
    {
        AddIncludes();

        if (!string.IsNullOrEmpty(spec.Sort))
        {
            switch (spec.Sort)
            {
                case "dateasc":
                    AddOrderBy(o => o.CreatedAt);
                    break;

                case "datedesc":
                    AddOrderByDescending(o => o.CreatedAt);
                    break;

                case "totalasc":
                    AddOrderBy(o => o.Total);
                    break;

                case "totaldesc":
                    AddOrderByDescending(o => o.Total);
                    break;

                default:
                    AddOrderByDescending(o => o.CreatedAt);
                    break;
            }
        }
        else
        {
            AddOrderByDescending(o => o.CreatedAt);
        }

        ApplyPaging(
            spec.PageSize * (spec.pageIndex - 1),
            spec.PageSize
        );

        ApplyNoTracking();
    }

    public OrderSpecification(string buyerId)
        : base(o => o.BuyerId == buyerId)
    {
        ApplyNoTracking();
    }

    private void AddIncludes()
    {
        Include.Add(o => o.Items);
        IncludeStrings.Add("Items.Options");
    }
}