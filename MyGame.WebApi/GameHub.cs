using Microsoft.AspNetCore.SignalR;

namespace MyGame.WebApi
{
    public class GameHub : Hub<IGameClient>
    {
    }
}