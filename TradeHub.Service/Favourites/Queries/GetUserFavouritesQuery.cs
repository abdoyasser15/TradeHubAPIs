using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;
using TradHub.Core.Specifications.Favourite_Spec;

namespace TradeHub.Service.Favourites.Queries
{
    public record GetUserFavouritesQuery
    : IRequest<Pagination<ProductDto>>
    {
        public FavouriteSpecParams FavouriteSpec { get; set; }
        public GetUserFavouritesQuery(FavouriteSpecParams favouriteSpec)
        {
            FavouriteSpec = favouriteSpec;
        }
    }
}
