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
            try
            {
                _logger.LogInfo("Attempting to delete product with Id={Id}", request.ID);

                var product = await _unitOfWork.Repository<Product>().GetById(request.ID);

                if (product == null)
                {
                    _logger.LogWarn("Product with Id={Id} not found. Delete aborted.", request.ID);
                    return false;
                }
                _unitOfWork.Repository<Product>().DeleteAsync(product);
                var result = await _unitOfWork.CompleteAsync();
                var success =  result > 0;
                _logger.LogInfo("Delete operation for product Id={Id} success={Success}", request.ID, success);
                return success;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product with Id={Id}", request.ID);
                throw;
            }
        }
    }
}
