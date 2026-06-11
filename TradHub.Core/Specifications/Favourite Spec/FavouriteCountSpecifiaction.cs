using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity;

namespace TradHub.Core.Specifications.Favourite_Spec
{
    public class FavouriteCountSpecifiaction : BaseSpecification<Favourite>
    {
        public FavouriteCountSpecifiaction(FavouriteSpecParams specParams)
            : base(F => (string.IsNullOrEmpty(specParams.Search) || F.Product.Name!.ToLower().Contains(specParams.Search))
            && (string.IsNullOrEmpty(specParams.UserId) ||
                F.UserId == specParams.UserId) && (!specParams.ProductId.HasValue || F.ProductId == specParams.ProductId))
        {
            
        }
    }
}
