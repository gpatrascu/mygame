using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGame.WebApi.Controllers
{
    public class GameModel
    {
        public Guid Id { get; set; }
        public List<string> Players { get; set; }

        public static GameModel From(Game game)
        {
            return new GameModel()
            {
                Id = game.Id, Players = game.Players.Select(player => player.PlayerName).ToList()
            };
        }
    }
}