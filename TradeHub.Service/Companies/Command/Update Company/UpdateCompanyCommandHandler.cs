using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Service_Contract;

namespace TradeHub.Service.Companies.Command.Update_Company
{
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, CompanyDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;
        private readonly IImageService _imageService;

        public UpdateCompanyCommandHandler(IUnitOfWork unitOfWork , ILoggerManager logger,IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _imageService = imageService;
        }
        public async Task<CompanyDto?> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo("UpdateCompanyCommand started. CompanyId: {CompanyId}", request.Id);

                if (request.Id == Guid.Empty)
                    throw new ArgumentException("CompanyId must be a valid GUID.");

                var dto = request.Company;

                if (dto is null)
                    throw new ArgumentNullException(nameof(request.Company));

                var companyRepo = _unitOfWork.Repository<Company>();
                var company = await companyRepo.GetById(request.Id);

                if (company is null)
                {
                    _logger.LogWarn("Company not found. CompanyId: {CompanyId}", request.Id);
                    return null;
                }

                var locationExists = await _unitOfWork.Repository<Location>()
                    .AnyAsync(l => l.Id == dto.LocationId);

                if (!locationExists)
                    throw new ArgumentException($"LocationId '{dto.LocationId}' is invalid.");

                var businessTypeExists = await _unitOfWork.Repository<BusinessType>()
                    .AnyAsync(bt => bt.BusinessTypeId == dto.BusinessTypeId);

                if (!businessTypeExists)
                    throw new ArgumentException($"BusinessTypeId '{dto.BusinessTypeId}' is invalid.");

                if (!string.IsNullOrWhiteSpace(dto.TaxNumber))
                {
                    var duplicateTax = await _unitOfWork.Repository<Company>()
                        .AnyAsync(c => c.TaxNumber == dto.TaxNumber && c.CompanyId != request.Id);

                    if (duplicateTax)
                        throw new DuplicateNameException($"TaxNumber '{dto.TaxNumber}' already exists.");
                }

                company.BusinessName = dto.BusinessName.Trim();
                company.TaxNumber = dto.TaxNumber;
                company.BusinessTypeId = dto.BusinessTypeId;
                company.LocationId = dto.LocationId;

                if (dto.LogoUrl is not null)
                {
                    if (!string.IsNullOrWhiteSpace(company.LogoUrl))
                    {
                        _imageService.DeleteImage(company.LogoUrl);
                    }

                    company.LogoUrl = await _imageService.UploadImageAsync(
                        dto.LogoUrl,
                        "images/companies"
                    );
                }

                companyRepo.Update(company);
                await _unitOfWork.CompleteAsync();

                _logger.LogInfo("Company updated successfully. CompanyId: {CompanyId}", request.Id);

                return new CompanyDto
                {
                    ID = company.CompanyId.ToString(),
                    BusinessName = company.BusinessName,
                    BusinessTypeId = company.BusinessTypeId,
                    TaxNumber = company.TaxNumber,
                    LogoUrl = company.LogoUrl,
                    CreatedById = company.CreatedById,
                    LocationId = company.LocationId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating Company. CompanyId: {CompanyId}", request.Id);
                throw;
            }
        }
    }
}
