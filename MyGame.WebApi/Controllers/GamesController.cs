using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace MyGame.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IHubContext<GameHub, IGameClient> gamehub;
        private readonly ILogger logger;

        private static readonly List<Game> games = new()
        {
            new()
            {
                Id = Guid.NewGuid(), 
                Players = new List<string>
                {
                    "george", 
                    "valentina"
                }
            }, 
            new()
            {
                Id = Guid.NewGuid(), 
                Players = new List<string>
                {
                    "george2", 
                    "valentina2"
                }
            }
        };

        public GamesController(IHubContext<GameHub, IGameClient> gamehub, ILogger<GamesController> logger)
        {
            this.gamehub = gamehub;
            this.logger = logger;
        }

        [HttpPost("moves")]
        public async Task<IActionResult> PostMove([FromBody] GameMove gameMove)
        {
            await this.gamehub.Clients.All.ReceiveGameMove(gameMove);
            
            return new CreatedResult("", gameMove);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewGameModel newGame)
        {
            var item = new Game
            {
                Id = Guid.NewGuid(), 
                Players = new List<string>
                {
                    GetName(newGame)
                }
            };
            games.Add(item);

            return Created("", item);
        }

        private string GetName(NewGameModel newGameModel)
        {
            if (newGameModel.Name != null)
            {
                return newGameModel.Name;
            }
            
            return User.Claims
                .FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/idntity/claims/name")
                ?.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(games);
        }
    }
}
