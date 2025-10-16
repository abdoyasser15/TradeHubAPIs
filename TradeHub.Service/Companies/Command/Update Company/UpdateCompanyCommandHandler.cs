using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;

namespace TradeHub.Service.Companies.Command.Update_Company
{
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, CompanyToDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CompanyToDto?> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var companyRepo =  _unitOfWork.Repository<Company>();
            var company = await companyRepo.GetById(request.Id);
            if (company == null) return null;
            var dto = request.Company;
            var locationExists = await _unitOfWork.Repository<Location>()
                .AnyAsync(l => l.Id == dto.LocationId);
            var businessTypeExists = await _unitOfWork.Repository<BusinessType>()
                .AnyAsync(bt => bt.BusinessTypeId == dto.BusinessTypeId);
            if (!locationExists || !businessTypeExists) return null;
            company.BusinessName = dto.BusinessName;
            company.TaxNumber = dto.TaxNumber;
            company.BusinessTypeId = dto.BusinessTypeId;
            company.LocationId = dto.LocationId;
            company.LogoUrl = dto.LogoUrl;
            companyRepo.Update(company);
            await _unitOfWork.CompleteAsync();
            return new CompanyToDto
            {
                BusinessName = company.BusinessName,
                BusinessTypeId = company.BusinessTypeId,
                CreatedById = company.CreatedById,
                LocationId = company.LocationId,
                LogoUrl = company.LogoUrl,
                TaxNumber = company.TaxNumber
            };
        }
    }
}
