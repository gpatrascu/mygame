using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
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
            await this.cosmosClient.Add(game);
        }

        public async Task<IList<Game>> GetActiveGames()
        {
            var feedIterator = this.cosmosClient.Container.GetItemLinqQueryable<Game>()
                .ToFeedIterator();

            var games = new List<Game>();
            while (feedIterator.HasMoreResults)
            {
                games.AddRange(await feedIterator.ReadNextAsync());
            }

            return games;
        }
    }
}