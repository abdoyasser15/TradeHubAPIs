using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Entity;

namespace TradeHub.Service.Companies.Command.Update_Company
{
    public class UpdateCompanyLogoCommandHandler : IRequestHandler<UpdateCompanyLogoCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCompanyLogoCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(UpdateCompanyLogoCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Company>();
            var company = await repo.GetById(request.CompanyId);
            if (company == null) return false;
            company.LogoUrl = request.Logo;
            repo.Update(company);
            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}
