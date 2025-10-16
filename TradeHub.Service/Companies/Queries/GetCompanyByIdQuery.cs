﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;

namespace TradeHub.Service.Companies.Queries
{
   public record GetCompanyByIdQuery(Guid Id) : IRequest<CompanyDto>;
}
