using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace MyGame.WebApi.Tests
{
    public class MyGameWebApiTests : IClassFixture<WebApplicationFactory<Startup>>

    {
        private readonly ITestOutputHelper testOutputHelper;

        private WebApplicationFactory<Startup> factory;
        private HttpClient httpClient;

        public MyGameWebApiTests(ITestOutputHelper testOutputHelper, WebApplicationFactory<Startup> factory)
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
            var games = await httpClient.GetStringAsync("/Games");
            testOutputHelper.WriteLine(games);
        }

        // [Fact]
        // public async Task ShowUserInfo()
        // {
        //     var token = "eyJhbGciOiJSUzI1NiIsImtpZCI6IkQ3MTk2RjBERjU5OUUxMzQ0QjVBRkQ4MDdDNTJEOUYzNEEwMEVCOEJSUzI1NiIsInR5cCI6ImF0K2p3dCIsIng1dCI6IjF4bHZEZldaNFRSTFd2MkFmRkxaODBvQTY0cyJ9.eyJuYmYiOjE2MDg0ODI4ODYsImV4cCI6MTYwODQ4NjQ4NiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTEwMCIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjUxMDAvcmVzb3VyY2VzIiwiY2xpZW50X2lkIjoibXlnYW1lIiwic3ViIjoiNTE4MUU5OTc3NDQxMEEwNzJGNzVGRDkyNUJCMTYyRThCQUY0OEEyNjNGNjAyMERFOTVENTU2RDQ2QzUzQTA0NCIsImF1dGhfdGltZSI6MTYwODQ4Mjg4NSwiaWRwIjoiRmFjZWJvb2siLCJqdGkiOiIwNTIzQ0QwRUMxMDc2Nzc5QjYyRDVCNjBCNzQxQTU0MyIsInNpZCI6IjMwRDY3REFEOTY2MTFDRkQ2ODREM0NCQjY0QzNDODU2IiwiaWF0IjoxNjA4NDgyODg2LCJzY29wZSI6WyJvcGVuaWQiLCJwcm9maWxlIiwibXlnYW1lIl0sImFtciI6WyJleHRlcm5hbCJdfQ.IfXSS2AhL70VSI30aAURWDdsWnlWieFgA-yU9tcYaANYRQ-NzrSAw_M6aNTuyAXsgxTOat693UlMuE3-I1otxSZXKWMbvdIqB-aP3zhbnBi3xTj192TT9Rru_wUFIFmXkAMJt7QoiDJZtvUfB2DNuKLIrCZj3ciMHa2NSEmyiVgXqqCQTFILU3eo_XcuuQd4TFRrP1x91R7YunyfCi-FokqeBv7jw2JPGQK5fjleI-T09ueioF0wT-iOowG3fMLfi7hd3WNNpgrmwWbGa3ofCtHs0FK8A3OtAL4LzH7Q2GiEmuZnYv6iTxhHQvWXxYoQELq_Kfus3flIUW_x4iAG8A";
        //     await PrintUserClaims(token);
        // }

        [Fact]
        public async Task PostGameTest()
        {
            var postNewGame = await httpClient.PostAsJsonAsync("/Games", new {Name = "George"});

            testOutputHelper.WriteLine((await postNewGame.Content.ReadAsStringAsync()));

            var postResult = await postNewGame.Content.ReadFromJsonAsync<JsonElement>();

            var value = postResult.GetProperty("id");

            Assert.NotNull(value);
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