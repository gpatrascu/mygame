using System;
using System.Collections.Generic;

namespace MyGame.WebApi.Controllers
{
    public class Game
    {
        public Game(string id, string ownerId, IList<Player> players)
        {
            Players = players;
            Id = id;
            OwnerId = ownerId;
        }

        public IList<Player> Players { get; }

        public string Id { get; }
        public string OwnerId { get; }

        public static Game CreateFor(string ownerId, string playerName)
        {
            IList<Player> players = new List<Player>
            {
                new(ownerId, playerName)
            };
            return new Game(Guid.NewGuid().ToString(), ownerId, players);
        }

        public void AddPlayer(Player player)
        {
            this.Players.Add(player);
        }
    }

    public class Player
    {
        public Player(string playerId, string playerName)
        {
            PlayerId = playerId;
            PlayerName = playerName;
        }

        public string PlayerId { get; }
        public string PlayerName { get; }
    }
}