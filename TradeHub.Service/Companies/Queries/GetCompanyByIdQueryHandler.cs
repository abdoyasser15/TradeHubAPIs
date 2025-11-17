using AutoMapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
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
using TradHub.Core.Service_Contract;
using TradHub.Core.Specifications.Company_Spec;

namespace TradeHub.Service.Companies.Queries
{
    public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public GetCompanyByIdQueryHandler(IUnitOfWork unitOfWork , IMapper mapper , ILoggerManager logger)
        {
            this.unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo("Fetching company with Id={Id}", request.Id);

                var spec = new CompanyWithBusinessTypeSpecification(request.Id);

                var company = await unitOfWork.Repository<Company>().GetWithSpecAsync(spec);

                if (company is null)
                {
                    _logger.LogWarn("Company with Id={Id} not found", request.Id);
                    return null;
                }
                _logger.LogInfo("Company with Id={Id} fetched successfully", request.Id);
                var mappedCompany = _mapper.Map<CompanyDto>(company);
                return mappedCompany;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error while fetching company with Id={Id}", request.Id);
                throw;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while fetching companies");
                throw;
            }
        }
    }
}
