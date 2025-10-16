using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHub.Service.Companies.Command.Delete_Company
{
    public class DeleteCompanyCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeleteCompanyCommand(Guid id)
        {
            Id = id;
        }
    }
}
