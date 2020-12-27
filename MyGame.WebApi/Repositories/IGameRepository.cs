using System.Collections.Generic;
using System.Threading.Tasks;
using MyGame.WebApi.Controllers;

namespace MyGame.WebApi.Repositories
{
    public interface IGameRepository
    {
        Task Add(Game game);
        Task<IList<Game>> GetActiveGames();
    }
}