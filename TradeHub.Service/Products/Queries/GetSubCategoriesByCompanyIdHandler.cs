using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Specifications.Product_Spec;

namespace TradeHub.Service.Products.Queries
{
    public class GetSubCategoriesByCompanyIdHandler : IRequestHandler<GetSubCategoriesByCompanyIdQuery, IReadOnlyList<SubCategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSubCategoriesByCompanyIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<SubCategoryDto>> Handle(GetSubCategoriesByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new ProductsByCompanyWithSubCategorySpecification(request.CompanyId);

            var products = await _unitOfWork
                .Repository<Product>()
                .GetAllSpecificationsAsync(spec);

            var subCategories = products
                .Where(p => p.SubCategory != null)
                .GroupBy(p => new { p.SubCategory.SubCategoryId, p.SubCategory.Name })
                .Select(g => new SubCategoryDto
                {
                    Id = g.Key.SubCategoryId,
                    Name = g.Key.Name ?? string.Empty
                })
                .ToList();

            return subCategories;
        }
    }
}
