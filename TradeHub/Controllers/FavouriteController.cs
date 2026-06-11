using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TradeHub.Service.Favourites.Command;
using TradeHub.Service.Favourites.Queries;
using TradHub.Core.Dtos;
using TradHub.Core.Specifications.Favourite_Spec;

namespace TradeHub.Controllers
{
    [Authorize]
    public class FavouriteController : BaseApiController
    {
        private readonly IMediator _mediator;

        public FavouriteController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductDto>>> GetUserFavourites(
            [FromQuery] FavouriteSpecParams favouriteSpec)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            favouriteSpec.UserId = userId;

            var result = await _mediator.Send(new GetUserFavouritesQuery(favouriteSpec));

            return Ok(result);
        }
        [HttpPost("toggle/{productId:int}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ToggleFavourite(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var isFavourite = await _mediator.Send(new ToggleFavouriteCommand(userId, productId));

            return Ok(new
            {
                success = true,
                productId,
                message = isFavourite ? "Added Successfully" : "Removed Successfully"
            });
        }
    }
}
