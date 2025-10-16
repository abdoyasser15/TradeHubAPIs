using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Entity;

namespace TradeHub.Service.Companies.Command.Delete_Company
{
    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Company>();
            var company = await repo.GetById(request.Id);
            if (company is null) return false;
            repo.DeleteAsync(company);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }
    }
}
