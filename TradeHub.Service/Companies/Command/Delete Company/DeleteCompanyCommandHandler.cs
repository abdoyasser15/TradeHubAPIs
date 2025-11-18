using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Entity;
using TradHub.Core.Service_Contract;

namespace TradeHub.Service.Companies.Command.Delete_Company
{
    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;

        public DeleteCompanyCommandHandler(IUnitOfWork unitOfWork , ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo("DeleteCompanyCommand started. CompanyId: {CompanyId}", request.Id);
                var repo = _unitOfWork.Repository<Company>();
                var company = await repo.GetById(request.Id);
                if (company is null) return false;
                repo.DeleteAsync(company);
                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                {
                    _logger.LogInfo("Company deleted successfully. CompanyId: {CompanyId}", request.Id);
                    return true;
                }
                else
                {
                    _logger.LogWarn("Delete operation completed but no rows were affected. CompanyId: {CompanyId}", request.Id);
                    return false;
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred while deleting company with Id: {CompanyId}", request.Id);
                throw;
            }
        }
    }
}
