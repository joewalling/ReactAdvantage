using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using ReactAdvantage.Api;
using System.Net.Http;
using System.Text;
using Xunit;

namespace ReactAdvantage.Tests.Integration.Api.Controllers
{
    public class GraphQLControllerShould
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public GraphQLControllerShould()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Test")
                .UseStartup<Startup>()
            );
            _client = _server.CreateClient();
        }

        [Fact]
        public async void ReturnTasks()
        {
            // Given
            var query = @"{
                ""query"": ""query { tasks { id name description } }""
            }";
            var content = new StringContent(query, Encoding.UTF8, "application/json");

            // When
            var response = await _client.PostAsync("/graphql", content);

            // Then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Create a great looking user interface", responseString);
            Assert.Equal(
                "{\"data\":{\"tasks\":[{\"id\":1,\"name\":\"Create UI\",\"description\":\"Create a great looking user interface\"},{\"id\":2,\"name\":\"Create Business logic\",\"description\":\"Create the business logic\"},{\"id\":3,\"name\":\"Create login form\",\"description\":\"Create a great looking login form\"},{\"id\":4,\"name\":\"Create logic for login\",\"description\":\"Create the logic for the login form\"}]}}",
                responseString);
        }
    }
}
