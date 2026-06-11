using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Repository;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Specifications.Favourite_Spec;

namespace TradeHub.Service.Favourites.Queries
{
    public class GetUserFavouritesQueryHandler : IRequestHandler<GetUserFavouritesQuery, Pagination<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserFavouritesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Pagination<ProductDto>> Handle(GetUserFavouritesQuery request, CancellationToken cancellationToken)
        {
            var spec = new FavouriteSpecifiaction(request.FavouriteSpec);

            var Favourites = await _unitOfWork.Repository<Favourite>().GetAllSpecificationsAsync(spec);

            var countSpec= new FavouriteCountSpecifiaction(request.FavouriteSpec);

            var count = await _unitOfWork.Repository<Favourite>().CountAsync(countSpec);

            var data = Favourites.Select(f => new ProductDto
            {
                Id = f.Product.Id,
                Name = f.Product.Name!,
                Description = f.Product.Description!,
                ImageUrl = f.Product.ImageUrl,
                Price = f.Product.Price,
                Quantity = f.Product.Quantity,
                SubCategoryId = f.Product.SubCategoryId,
                SubCategoryName = f.Product.SubCategory.Name,
                CompanyId = f.Product.CompanyId,
                CompanyName = f.Product.Company.BusinessName,
                Attributes = f.Product.ProductAttributes.Select(a => new productattributedto
                {
                    Id = a.Id,
                    CategoryAttributeId = a.CategoryAttributeId,
                    CategoryAttributeName = a.CategoryAttribute.Name,
                    Value = a.Value!
                }).ToList(),
                IsFavourite = true
            }).ToList();

            return new Pagination<ProductDto>(
                request.FavouriteSpec.PageIndex,
                request.FavouriteSpec.PageSize,
                count,
                data
            );
        }
    }
}
