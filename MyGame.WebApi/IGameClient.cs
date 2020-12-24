using System.Threading.Tasks;

namespace MyGame.WebApi
{
    public interface IGameClient
    {
        Task ReceiveGameMove(GameMove move);
    }
}