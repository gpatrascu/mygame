using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MyGame.WebApi.Repositories;

namespace MyGame.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IHubContext<GameHub, IGameClient> gamehub;
        private readonly ILogger logger;

        private readonly IGameRepository gameRepository;

        public GamesController(IHubContext<GameHub, IGameClient> gamehub, ILogger<GamesController> logger,
            IGameRepository gameRepository)
        {
            this.gamehub = gamehub;
            this.logger = logger;
            this.gameRepository = gameRepository;
        }

        [HttpPost("{gameId}/players")]
        public async Task<IActionResult> PostPlayer(string gameId, [FromBody] PlayerModel player)
        {
            var game = await gameRepository.GetById(gameId);
            game.AddPlayer(new Player(GetUserId(), player.Name));

            await gameRepository.Update(game);
            var postPlayer = GameModel.From(game);
            return Created("", postPlayer);
        }
        
        [HttpDelete("{gameId}/players/{playerId}")]
        public async Task<IActionResult> RemovePlayer(string gameId, string playerId)
        {
            var game = await gameRepository.GetById(gameId);
            game.RemovePlayer(playerId);
            await gameRepository.Update(game);
            var gameModel = GameModel.From(game);
            return Ok(gameModel);
        }
        
        
        [HttpPost("moves")]
        public async Task<IActionResult> PostMove([FromBody] GameMove gameMove)
        {
            await gamehub.Clients.All.ReceiveGameMove(gameMove);

            return new CreatedResult("", gameMove);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewGameModel newGame)
        {
            var game = Game.CreateFor(GetUserId(), GetUserName(newGame));
            await gameRepository.Add(game);
            return Created("", GameModel.From(game));
        }

        private string GetUserId()
        {
            return GetClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        }

        private string GetUserName(NewGameModel newGameModel)
        {
            if (newGameModel.Name != null)
            {
                return newGameModel.Name;
            }

            return GetClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
        }

        private string GetClaim(string claimName)
        {
            return User.Claims
                .FirstOrDefault(claim => claim.Type == claimName)
                ?.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var activeGames = await gameRepository.GetActiveGames();
            return Ok(activeGames.Select(GameModel.From));
        }
    }
}