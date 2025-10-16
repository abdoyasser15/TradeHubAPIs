using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Repository;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Repository_Contract;
using TradHub.Core.Specifications.Company_Spec;

namespace TradeHub.Service.Companies.Queries
{
    public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetCompanyByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new CompanyWithBusinessTypeSpecification(request.Id);
            var company = await unitOfWork.Repository<Company>().GetWithSpecAsync(spec);
            if (company is null)
                return null;
            var companyDto = new CompanyDto
            {
                ID = company.CompanyId,
                BusinessName = company.BusinessName,
                BusinessTypeId = company.BusinessTypeId,
                TaxNumber = company.TaxNumber,
                LocationId = company.LocationId,
                LogoUrl = company.LogoUrl,
                CreatedById = company.CreatedById,
                LocationName = company.Location != null ? company.Location.Name : string.Empty,
                BusinessTypeName = company.BusinessType != null ? company.BusinessType.Name : string.Empty
            };
            return companyDto;
        }
    }
}
