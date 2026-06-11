using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;

namespace TradeHub.Service.Companies.Command.Create_Company
{
    public class CreateCompanyCommand : IRequest<CompanyToDto>
    {
        public CreateCompanyDto Company { get; set; }
        public CreateCompanyCommand(CreateCompanyDto company)
        {
            Company = company;
        }
    }
}
