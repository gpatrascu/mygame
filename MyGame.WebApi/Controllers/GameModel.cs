using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGame.WebApi.Controllers
{
    public class GameModel
    {
        public Guid Id { get; set; }
        public List<PlayerModel> Players { get; set; }
        public string OwnerId { get; set; }

        public static GameModel From(Game game)
        {
            return new()
            {
                OwnerId = game.OwnerId,
                Id = Guid.Parse(game.Id), Players = game.Players.Select(player => new PlayerModel()
                {
                    Id = player.PlayerId, Name = player.PlayerName
                }).ToList()
            };
        }
    }

    public class PlayerModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}