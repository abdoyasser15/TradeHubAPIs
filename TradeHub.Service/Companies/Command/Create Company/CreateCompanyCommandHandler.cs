using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Repository_Contract;

namespace TradeHub.Service.Companies.Command.Create_Company
{
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, CompanyToDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCompanyCommandHandler(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CompanyToDto?> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Company;
            var company = new Company
            {
                BusinessName = dto.BusinessName,
                BusinessTypeId = dto.BusinessTypeId,
                TaxNumber = dto.TaxNumber,
                LocationId = dto.LocationId,
                LogoUrl = dto.LogoUrl,
                CreatedById = dto.CreatedById
            };
            var locationExists = await _unitOfWork.Repository<Location>()
                .AnyAsync(l => l.Id == dto.LocationId);
            var businessTypeExists = await _unitOfWork.Repository<BusinessType>()
                .AnyAsync(bt => bt.BusinessTypeId == dto.BusinessTypeId);
            if (!locationExists||!businessTypeExists)
                return null;
            var createdCompany = _unitOfWork.Repository<Company>().AddAsync(company);
            await _unitOfWork.CompleteAsync();
            var companyToReturn  = new CompanyToDto
            {
                BusinessName = company.BusinessName,
                BusinessTypeId = company.BusinessTypeId,
                TaxNumber = company.TaxNumber,
                LocationId = company.LocationId,
                LogoUrl = company.LogoUrl,
                CreatedById = company.CreatedById
            };
            return companyToReturn;
        }
    }
}
