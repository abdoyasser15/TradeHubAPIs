using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHub.Service.Products.Command.Delete_product
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public int ID { get; set; }
        public DeleteProductCommand(int id)
        {
            ID = id;
        }
    }
}
