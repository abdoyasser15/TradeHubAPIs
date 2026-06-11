using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Entity;

namespace TradeHub.Service.Favourites.Command
{
    public class ToggleFavouriteCommandHandler : IRequestHandler<ToggleFavouriteCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ToggleFavouriteCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(ToggleFavouriteCommand request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.Repository<Favourite>();

            var favourite = await repository.FirstOrDefaultAsync(F => F.UserId == request.UserId && F.ProductId == request.ProductId);

            if (favourite != null)
            {
                repository.DeleteAsync(favourite);
            }
            else
            {
               await repository.AddAsync(new Favourite
                {
                    UserId = request.UserId,
                    ProductId = request.ProductId,
                    CreatedAt = DateTime.UtcNow
                });
            }
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
