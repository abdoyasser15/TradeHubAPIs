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

namespace TradeHub.Service.Companies.Queries
{
    public class GetCompaniesCreatedByUserIdQueryHandler : IRequestHandler<GetCompaniesCreatedByUserIdQuery, IReadOnlyList<CompanyToDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;

        public GetCompaniesCreatedByUserIdQueryHandler(IUnitOfWork unitOfWork , ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<IReadOnlyList<CompanyToDto>> Handle(GetCompaniesCreatedByUserIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInfo("Fetching companies created by user {UserId}", request.UserId);
            try
            {
                if (string.IsNullOrWhiteSpace(request.UserId))
                {
                    _logger.LogWarn("GetCompaniesCreatedByUserIdQuery failed: Invalid UserId");
                    return Array.Empty<CompanyToDto>();
                }
                var repo = _unitOfWork.Repository<Company>();
                var companies = await repo.ListAsync(C => C.CreatedById == request.UserId);
                if (!companies.Any())
                {
                    _logger.LogInfo("No companies found for user {UserId}", request.UserId);
                    return Array.Empty<CompanyToDto>();
                }
                var companyDtos = companies.Select(c => new CompanyToDto
                {
                    BusinessName = c.BusinessName,
                    BusinessTypeId = c.BusinessTypeId,
                    CreatedById = c.CreatedById,
                    LocationId = c.LocationId,
                    LogoUrl = c.LogoUrl,
                    TaxNumber = c.TaxNumber
                }).ToList();
                _logger.LogInfo("Found {Count} companies for user {UserId}", companyDtos.Count, request.UserId);
                return companyDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching companies for user {UserId}", request.UserId);
                throw;
            }
        }
    }
}
