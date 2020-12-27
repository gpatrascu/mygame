using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MyGame.WebApi.Controllers;
using MyGame.WebApi.Repositories;

namespace MyGame.WebApi.Tests
{
    public class JoinWorkflowWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(IMyGameCosmosClient));
                services.Remove(descriptor);

                var myGameCosmosClient = new MyGameCosmosClient("MyGame_Join").Initialize().Result;
                myGameCosmosClient.ResetContainer().Wait();
                services.AddSingleton<IMyGameCosmosClient>(myGameCosmosClient);
            });
        }
    }
}