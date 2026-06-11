using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Entity;
using TradHub.Core.Service_Contract;

namespace TradeHub.Service.Products.Command.Delete_product
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork , ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInfo("Attempting to delete product with Id={Id}", request.ID);

            var product = await _unitOfWork
                .Repository<Product>()
                .GetById(request.ID);

            if (product is null)
            {
                _logger.LogWarn("Product with Id={Id} not found. Delete aborted.", request.ID);
                throw new KeyNotFoundException($"Product with Id {request.ID} not found");
            }

                 _unitOfWork
                .Repository<Product>()
                .DeleteAsync(product);

            var affectedRows = await _unitOfWork.CompleteAsync();

            if (affectedRows <= 0)
                throw new InvalidOperationException("Delete operation failed");

            _logger.LogInfo("Product with Id={Id} deleted successfully", request.ID);
            return true;
        }
    }
}
