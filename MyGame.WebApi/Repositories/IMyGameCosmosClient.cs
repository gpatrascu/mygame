using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace MyGame.WebApi.Controllers
{
    public interface IMyGameCosmosClient
    {
        Container Container { get; }
        Task Add(Game game);
    }
}