using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;

namespace TradeHub.Service.Companies.Queries
{
    public class GetCompaniesCreatedByUserIdQueryHandler : IRequestHandler<GetCompaniesCreatedByUserIdQuery, IReadOnlyList<CompanyToDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompaniesCreatedByUserIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<CompanyToDto>> Handle(GetCompaniesCreatedByUserIdQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Company>();
            var companies = await repo.ListAsync(C=>C.CreatedById==request.UserId);
            var companyDtos = companies.Select(c => new CompanyToDto
            {
                BusinessName = c.BusinessName,
                BusinessTypeId = c.BusinessTypeId,
                CreatedById = c.CreatedById,
                LocationId = c.LocationId,
                LogoUrl = c.LogoUrl,
                TaxNumber = c.TaxNumber
            }).ToList();
            return companyDtos;
        }
    }
}
