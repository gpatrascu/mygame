using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyGame.WebApi.Tests
{
    public static class IdentityOperations
    {
        public static async Task<string> Authenticate(string username)
        {
            var httpClientForAuth =
                new HttpClient {BaseAddress = new Uri("https://localhost:5100")};

            var httpResponseMessage = await httpClientForAuth.PostAsync("/connect/token", new FormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>("client_id", "mygame"),
                    new KeyValuePair<string, string>("client_secret", "secret"),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", username),
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