using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;

namespace TradeHub.Service.Companies.Command.Update_Company
{
    public class UpdateCompanyCommand : IRequest<CompanyToDto>
    {
        public Guid Id { get; set; }
        public CompanyToDto Company { get; set; }
    }
}
