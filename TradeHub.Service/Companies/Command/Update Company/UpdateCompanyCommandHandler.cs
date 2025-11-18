using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Service_Contract;

namespace TradeHub.Service.Companies.Command.Update_Company
{
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, CompanyToDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;

        public UpdateCompanyCommandHandler(IUnitOfWork unitOfWork , ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<CompanyToDto?> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo("UpdateCompanyCommand started. CompanyId: {CompanyId}", request.Id);

                var companyRepo = _unitOfWork.Repository<Company>();
                var company = await companyRepo.GetById(request.Id);

                if (company is null)
                {
                    _logger.LogWarn("Company not found. CompanyId: {CompanyId}", request.Id);
                    return null;
                }

                var dto = request.Company;

                var locationExists = await _unitOfWork.Repository<Location>()
                    .AnyAsync(l => l.Id == dto.LocationId);

                var businessTypeExists = await _unitOfWork.Repository<BusinessType>()
                    .AnyAsync(bt => bt.BusinessTypeId == dto.BusinessTypeId);

                if (!locationExists || !businessTypeExists)
                {
                    _logger.LogWarn(
                        "Foreign key validation failed. CompanyId: {CompanyId}, LocationExists: {LocationExists}, BusinessTypeExists: {BusinessTypeExists}",
                        request.Id,
                        locationExists,
                        businessTypeExists
                    );
                    return null;
                }

                company.BusinessName = dto.BusinessName;
                company.TaxNumber = dto.TaxNumber;
                company.BusinessTypeId = dto.BusinessTypeId;
                company.LocationId = dto.LocationId;
                company.LogoUrl = dto.LogoUrl;

                companyRepo.Update(company);
                await _unitOfWork.CompleteAsync();

                _logger.LogInfo("Company updated successfully. CompanyId: {CompanyId}", request.Id);

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
            catch (Exception ex)
            {
                _logger.LogError(ex,"An error occurred while updating Company. CompanyId: {CompanyId}",request.Id);
                throw;
            }
        }
    }
}
