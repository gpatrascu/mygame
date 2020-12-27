using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using MyGame.WebApi.Controllers;
using Xunit;
using Xunit.Abstractions;

namespace MyGame.WebApi.Tests
{
    public class MyGameWebApi_JoinTests : IClassFixture<WebApplicationFactory<Startup>>

    {
        private readonly ITestOutputHelper testOutputHelper;

        private WebApplicationFactory<Startup> factory;
        private HttpClient httpClient;

        public MyGameWebApi_JoinTests(ITestOutputHelper testOutputHelper, WebApplicationFactory<Startup> factory)
        {
            this.testOutputHelper = testOutputHelper;
            this.factory = factory;
            // httpClient = new HttpClient {BaseAddress = new Uri("https://localhost:5001")};

            httpClient = factory.CreateClient();
            var token = Authenticate().Result;
            PrintUserClaims(token).Wait();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        [Fact]
        public async Task GetGamesTest()
        {
            var games = await httpClient.GetFromJsonAsync<IList<GameModel>>("/Games");

            foreach (var game in games)
            {
                testOutputHelper.WriteLine(game.Id.ToString());
                foreach (var gamePlayer in game.Players)
                {
                    testOutputHelper.WriteLine(gamePlayer);
                }
            }
        }

        [Fact]
        public async Task PostGameTest()
        {
            var postNewGame = await httpClient.PostAsJsonAsync("/Games", new {Name = "George"});

            testOutputHelper.WriteLine((await postNewGame.Content.ReadAsStringAsync()));

            var postResult = await postNewGame.Content.ReadFromJsonAsync<JsonElement>();

            postResult.GetProperty("id");
        }
        
        [Fact]
        public async Task JoinGameTest()
        {
            var postNewGame = await httpClient.PostAsJsonAsync("/Games", new {Name = "George"});

            testOutputHelper.WriteLine((await postNewGame.Content.ReadAsStringAsync()));

            var postResult = await postNewGame.Content.ReadFromJsonAsync<JsonElement>();

            postResult.GetProperty("id");
        }

        private async Task PrintUserClaims(string token)
        {
            HttpClient httpClientForAuth =
                new HttpClient {BaseAddress = new Uri("https://localhost:5100")};
            httpClientForAuth.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var stringAsync = await httpClientForAuth.GetStringAsync("/connect/userinfo");

            testOutputHelper.WriteLine(stringAsync);
        }

        private async Task<string> Authenticate()
        {
            HttpClient httpClientForAuth =
                new HttpClient {BaseAddress = new Uri("https://localhost:5100")};

            var httpResponseMessage = await httpClientForAuth.PostAsync("/connect/token", new FormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>("client_id", "mygame"),
                    new KeyValuePair<string, string>("client_secret", "secret"),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", "george.patrascu@yahoo.com"),
                    new KeyValuePair<string, string>("password", "password"),
                    new KeyValuePair<string, string>("scope", "openid profile mygame email"),
                    new KeyValuePair<string, string>("response_type", "code"),
                }));

            var jObject = await httpResponseMessage.Content.ReadFromJsonAsync<JsonElement>();

            var accessToken = jObject.GetProperty("access_token").ToString();
            ////testOutputHelper.WriteLine(accessToken);

            return accessToken;
        }
    }
}