using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Mvc.Testing;
using MyGame.WebApi.Controllers;
using Xunit;
using Xunit.Abstractions;

namespace MyGame.WebApi.Tests
{
    public class JoinGameTests : IClassFixture<JoinWorkflowWebApplicationFactory>
    {
        private readonly ITestOutputHelper testOutputHelper;

        private WebApplicationFactory<Startup> factory;
        private HttpClient httpClient;

        public JoinGameTests(ITestOutputHelper testOutputHelper, JoinWorkflowWebApplicationFactory factory)
        {
            this.testOutputHelper = testOutputHelper;
            this.factory = factory;
            httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task JoinTest()
        {
            SetAuthorizationHeader(await IdentityOperations.Authenticate("george.patrascu@yahoo.com"));
            var game = await CreateNewGame();
            SetAuthorizationHeader(await IdentityOperations.Authenticate("valentina.patrascu@yahoo.com"));
            await JoinGame(game);
        }

        private async Task JoinGame(GameModel gameModel)
        {
            var postNewGame = await httpClient.PostAsJsonAsync($"/Games/{gameModel.Id}/players", new {Name = "Valentina"});
            Assert.Equal(HttpStatusCode.Created, postNewGame.StatusCode);
            var game = await postNewGame.Content.ReadFromJsonAsync<GameModel>();
            Assert.Equal(2, game.Players.Count);
        }

        private void SetAuthorizationHeader(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task<GameModel> CreateNewGame()
        {
            var postNewGame = await httpClient.PostAsJsonAsync("/Games", new {Name = "George"});

            testOutputHelper.WriteLine(await postNewGame.Content.ReadAsStringAsync());

            var postResult = await postNewGame.Content.ReadFromJsonAsync<GameModel>();

            return postResult;
        }

        private async Task PrintUserClaims(string token)
        {
            HttpClient httpClientForAuth =
                new HttpClient {BaseAddress = new Uri("https://localhost:5100")};
            httpClientForAuth.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var stringAsync = await httpClientForAuth.GetStringAsync("/connect/userinfo");

            testOutputHelper.WriteLine(stringAsync);
        }
    }
}