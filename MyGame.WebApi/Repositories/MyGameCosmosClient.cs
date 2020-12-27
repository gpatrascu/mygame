using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using MyGame.WebApi.Controllers;

namespace MyGame.WebApi.Repositories
{
    public class MyGameCosmosClient : IMyGameCosmosClient
    {
        public Container Container { get; private set; }

        public async Task Add(Game game)
        {
            await Container.CreateItemAsync(game);
        }

        public async Task<MyGameCosmosClient> Initialize()
        {
            var cosmosClient = new CosmosClientBuilder("https://mygame.documents.azure.com:443/",
                    "Q8Kv0aKC8iZS41BUWAKzjbPokUBQObmQR48LHlZN62kfYUBDjsQDG5YrDAQmy7ToprJ566eZVw4lBvt5DiYLxQ==")
                .WithSerializerOptions(new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                })
                .Build();
            
            var databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync("Games");
            var containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync("MyGame", "/ownerId");

            Container = containerResponse.Container;

            return this;
        }
    }
}