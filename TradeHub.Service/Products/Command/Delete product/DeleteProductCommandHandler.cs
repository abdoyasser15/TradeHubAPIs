using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Entity;

namespace TradeHub.Service.Products.Command.Delete_product
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Repository<Product>().GetById(request.ID);
            if (product == null)
                return false;
            _unitOfWork.Repository<Product>().DeleteAsync(product);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }
    }
}
