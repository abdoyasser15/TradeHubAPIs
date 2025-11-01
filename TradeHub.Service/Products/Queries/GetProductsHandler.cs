using AutoMapper;
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
    public class GetProductsHandler : IRequestHandler<GetProductsQuery, Pagination<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsHandler(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Pagination<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var spec = new ProductSpecification(request.ProductSepc);
            var product =  await _unitOfWork.Repository<Product>().GetAllSpecificationsAsync(spec);
            var countSpec = new ProductCountSpecifications(request.ProductSepc);
            var totalItems = await _unitOfWork.Repository<Product>().CountAsync(countSpec);
            var mappedProducts = product.Select(p => _mapper.Map<ProductDto>(p)).ToList();
            var result = new Pagination<ProductDto>(request.ProductSepc.pageIndex, request.ProductSepc.PageSize, totalItems, mappedProducts);
            return result;
        }
    }
}
