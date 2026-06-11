using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHub.Service.Favourites.Command
{
    public record ToggleFavouriteCommand(string UserId, int ProductId) : IRequest<bool>;
}
