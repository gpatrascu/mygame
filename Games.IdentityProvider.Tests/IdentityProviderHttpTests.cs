using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Games.IdentityProvider.Tests
{
    public class IdentityProviderHttpTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public IdentityProviderHttpTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task GetAccessToken()
        {
            HttpClient httpClient = 
                new HttpClient {BaseAddress = new Uri("https://localhost:5100")};

            var httpResponseMessage = await httpClient.PostAsync("/connect/token", new FormUrlEncodedContent(new []
            {
                new KeyValuePair<string, string>("client_id", "mygame"), 
                new KeyValuePair<string, string>("client_secret", "secret"), 
                new KeyValuePair<string, string>("grant_type", "password"), 
                new KeyValuePair<string, string>("username", "george.patrascu@yahoo.com"),
                new KeyValuePair<string, string>("password", "password"),
            }));

            var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();

            testOutputHelper.WriteLine(readAsStringAsync);
        }
    }
}
