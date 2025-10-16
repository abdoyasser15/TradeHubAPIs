using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Specifications.Company_Spec;

namespace TradeHub.Service.Companies.Queries
{
    public class GetCompaniesQuery : IRequest<Pagination<CompanyDto>>
    {
        public CompanySpecificationParams SpecParams { get; }

        public GetCompaniesQuery(CompanySpecificationParams specParams)
        {
            SpecParams = specParams;
        }
    }
}
