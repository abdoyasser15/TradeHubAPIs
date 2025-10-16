using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Specifications.Company_Spec;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TradeHub.Service.Companies.Queries
{
    public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, Pagination<CompanyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompaniesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Pagination<CompanyDto>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
        {
            var spec = new CompanyWithBusinessTypeSpecification(request.SpecParams);
            var companies = await _unitOfWork.Repository<Company>().GetAllSpecificationsAsync(spec);
            var countSpec = new CompanyWithCountSpecification(request.SpecParams);
            var totalItems = await _unitOfWork.Repository<Company>().CountAsync(countSpec);
            var mappedCompanies = companies.Select(c => new CompanyDto
            {
                ID = c.CompanyId,
                BusinessName = c.BusinessName,
                BusinessTypeId = c.BusinessTypeId,
                TaxNumber = c.TaxNumber,
                LocationId = c.LocationId,
                LogoUrl = c.LogoUrl,
                CreatedById = c.CreatedById,
                LocationName = c.Location != null ? c.Location.Name : string.Empty,
                BusinessTypeName = c.BusinessType != null ? c.BusinessType.Name : string.Empty
            }).ToList();
            var result = new Pagination<CompanyDto>(request.SpecParams.pageIndex, request.SpecParams.PageSize, totalItems, mappedCompanies);
            return result;
        }
    }
}
