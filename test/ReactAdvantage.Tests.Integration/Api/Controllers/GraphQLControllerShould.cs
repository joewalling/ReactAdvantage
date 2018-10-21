using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Text;
using IdentityModel.Client;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ReactAdvantage.Domain.Services;
using ReactAdvantage.IdentityServer.Startup;
using Xunit;

namespace ReactAdvantage.Tests.Integration.Api.Controllers
{
    public class GraphQLControllerShould
    {
        private readonly TestServer _identityServer;
        private readonly HttpMessageHandler _identityServerHttpHandler;
        private readonly TestServer _apiServer;
        private readonly HttpClient _apiClient;

        public GraphQLControllerShould()
        {
            _identityServer = new TestServer(new WebHostBuilder()
                .UseEnvironment("Test")
                .UseStartup<ReactAdvantage.IdentityServer.Startup.Startup>());
            _identityServerHttpHandler = _identityServer.CreateHandler();

            var authenticationOptionsSetter = (Action<IdentityServerAuthenticationOptions>)((options) =>
            {
                options.Authority = _identityServer.BaseAddress.ToString();
                options.ApiName = "ReactAdvantageApi";
                options.RequireHttpsMetadata = false;

                options.JwtBackChannelHandler = _identityServerHttpHandler;
                options.IntrospectionDiscoveryHandler = _identityServerHttpHandler;
                options.IntrospectionBackChannelHandler = _identityServerHttpHandler;
            });

            _apiServer = new TestServer(new WebHostBuilder()
                .UseEnvironment("Test")
                .ConfigureServices(c =>
                {
                    c.AddSingleton(authenticationOptionsSetter);
                    c.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                    c.AddScoped<ITenantProvider, FakeTenantProvider>();
                })
                .UseStartup<ReactAdvantage.Api.Startup>());
            _apiClient = _apiServer.CreateClient();
        }

        [Fact]
        public async void Return401Unauthorized()
        {
            // Given
            var query = @"{
                ""query"": ""query { tasks { id name description } }""
            }";
            var content = new StringContent(query, Encoding.UTF8, "application/json");

            // When
            var response = await _apiClient.PostAsync("/graphql", content);

            // Then
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.True(string.IsNullOrEmpty(responseString));
        }

        [Fact]
        public async void ReturnTasks()
        {
            // Given
            var tokenClient = new TokenClient(_identityServer.BaseAddress + "connect/token", "testClient", "secret", _identityServerHttpHandler);

            var query = @"{
                ""query"": ""query { tasks { id name description } }""
            }";
            var content = new StringContent(query, Encoding.UTF8, "application/json");

            // When
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync(ApiResources.ReactAdvantageApi);
            _apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await _apiClient.PostAsync("/graphql", content);

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
