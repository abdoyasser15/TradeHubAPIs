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
using TradHub.Core.Service_Contract;
using TradHub.Core.Specifications.Company_Spec;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TradeHub.Service.Companies.Queries
{
    public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, Pagination<CompanyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public GetCompaniesQueryHandler(IUnitOfWork unitOfWork , ILoggerManager logger , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Pagination<CompanyDto>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo("Fetching companies with filters: {@Filters}", request.SpecParams);

                var spec = new CompanyWithBusinessTypeSpecification(request.SpecParams);

                var companies = await _unitOfWork.Repository<Company>().GetAllSpecificationsAsync(spec);

                var countSpec = new CompanyWithCountSpecification(request.SpecParams);

                var totalItems = await _unitOfWork.Repository<Company>().CountAsync(countSpec);

                _logger.LogInfo("Fetched {Count} companies from DB", companies.Count);

                var mappedCompanies = _mapper.Map<List<CompanyDto>>(companies);

                var result = new Pagination<CompanyDto>(
                    request.SpecParams.pageIndex,
                    request.SpecParams.PageSize,
                    totalItems,
                    mappedCompanies
                );
                return result;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while fetching companies");
                throw;
            }
        }
    }
}
