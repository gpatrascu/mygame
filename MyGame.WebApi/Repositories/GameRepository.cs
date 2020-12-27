using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using MyGame.WebApi.Repositories;

namespace MyGame.WebApi.Controllers
{
    public class GameRepository : IGameRepository
    {
        private IMyGameCosmosClient cosmosClient;

        public GameRepository(IMyGameCosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }

        public async Task Add(Game game)
        {
            await cosmosClient.Add(game);
        }

        public async Task<IList<Game>> GetActiveGames()
        {
            var feedIterator = cosmosClient.Container.GetItemLinqQueryable<Game>()
                .ToFeedIterator();

            var games = new List<Game>();
            while (feedIterator.HasMoreResults)
            {
                games.AddRange(await feedIterator.ReadNextAsync());
            }

            return games;
        }

        public async Task<Game> GetById(string gameId)
        {
            try
            {
                return await cosmosClient.Container.ReadItemAsync<Game>(gameId, new PartitionKey(gameId));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task Update(Game game)
        {
            await cosmosClient.Container.UpsertItemAsync(game, new PartitionKey(game.Id));
        }
    }
}