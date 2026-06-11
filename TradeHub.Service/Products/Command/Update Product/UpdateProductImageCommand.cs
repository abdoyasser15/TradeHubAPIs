using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHub.Service.Products.Command.Update_Product
{
    public class UpdateProductImageCommand : IRequest<string?>
    {
        public int ProductId { get; set; }
        public IFormFile Image { get; set; } = default!;
    }
}
