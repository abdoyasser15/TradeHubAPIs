using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHub.Service.Companies.Command.Update_Company
{
    public record UpdateCompanyLogoCommand(Guid CompanyId, string Logo) : IRequest<bool>;
}
