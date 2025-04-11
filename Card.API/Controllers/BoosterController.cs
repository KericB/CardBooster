using CardBooster.Core.Commands;
using CardBooster.Core.Commands.Boosters;
using CardBooster.Core.Models;
using CardBooster.Core.Queries;
using CardBooster.Core.Queries.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Card.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoosterController : ControllerBase
    {
        private readonly ICommandAsyncHandler<OpenBoosterCommand> _openBoosterHandler;
        private readonly IQueryAsyncHandler<GetUserCardQuery, List<Cards>> _getUserCardsHandler;

        public BoosterController(
            ICommandAsyncHandler<OpenBoosterCommand> openBoosterHandler, 
            IQueryAsyncHandler<GetUserCardQuery, List<Cards>> getUserCardsHandler)
        {
            _openBoosterHandler = openBoosterHandler;
            _getUserCardsHandler = getUserCardsHandler;
        }

        [HttpPost("open")]

        public async Task<IActionResult> OpenBooster()
        {
            if(!int .TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                return BadRequest(new { Error = "Utilisateur non authentifié correctement"});
            }

            var command = new OpenBoosterCommand { UserId= userId };
            var result = await _openBoosterHandler.ExecuteAsync(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new {Error = result.ErrorMessage});
            }

            return Ok(result);
        }

        [HttpGet("cards")]

        public async Task<IActionResult> GetUserCards()
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                return BadRequest(new { Error = "Utilisateur non authentifié correctement" });
            }
            var query = new GetUserCardQuery { UserId = userId };
            var result = await _getUserCardsHandler.ExecuteAsync(query);
            if (!result.IsSuccess)
            {
                return BadRequest(new { Error = result.ErrorMessage });
            }
            return Ok(result.Content);
        }
    }
}
