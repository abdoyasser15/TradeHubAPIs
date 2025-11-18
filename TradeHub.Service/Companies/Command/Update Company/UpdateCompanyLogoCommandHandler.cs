using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Entity;
using TradHub.Core.Service_Contract;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TradeHub.Service.Companies.Command.Update_Company
{
    public class UpdateCompanyLogoCommandHandler : IRequestHandler<UpdateCompanyLogoCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;

        public UpdateCompanyLogoCommandHandler(IUnitOfWork unitOfWork , ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<bool> Handle(UpdateCompanyLogoCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInfo("UpdateCompanyLogoCommand started. CompanyId: {CompanyId}", request.CompanyId);
            try
            {
                if (request.CompanyId == Guid.Empty)
                {
                    _logger.LogWarn("UpdateCompanyLogoCommand failed: Invalid CompanyId");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(request.Logo))
                {
                    _logger.LogWarn("UpdateCompanyLogoCommand failed: Logo URL is null or empty");
                    return false;
                }
                var repo = _unitOfWork.Repository<Company>();
                var company = await repo.GetById(request.CompanyId);
                if (company == null)
                {
                    _logger.LogWarn("Company not found. CompanyId: {CompanyId}", request.CompanyId);
                    return false;
                }
                company.LogoUrl = request.Logo;
                repo.Update(company);
                var updated =  await _unitOfWork.CompleteAsync() > 0;
                if (updated)
                {
                    _logger.LogInfo("Company logo updated successfully. CompanyId: {CompanyId}", request.CompanyId);
                }
                else
                {
                    _logger.LogWarn("UpdateCompanyLogoCommand completed but no rows affected. CompanyId: {CompanyId}", request.CompanyId);
                }
                return updated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating company logo. CompanyId: {CompanyId}", request.CompanyId);
                throw;
            }
        }
    }
}
