using Microsoft.Extensions.Configuration;
using Moq;
using ReactAdvantage.Domain.Configuration;
using Xunit;

namespace ReactAdvantage.Tests.Unit.Domain.Configuration
{
    public class BaseUrlsShould
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly BaseUrls _baseUrls;

        public BaseUrlsShould()
        {
            //Given
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(x => x["BaseUrls:Api"]).Returns("http://api");
            _configurationMock.Setup(x => x["BaseUrls:IdentityServer"]).Returns("http://identity");
            _configurationMock.Setup(x => x["BaseUrls:GraphqlPlaygroundJsClient"]).Returns("http://graphqlPlayground");
            _configurationMock.Setup(x => x["BaseUrls:ReactClient"]).Returns("http://react");
            _configurationMock.Setup(x => x["BaseUrls:ReactClientLocal"]).Returns("http://reactLocal");

            _baseUrls = _configurationMock.Object.GetBaseUrls();
        }

        [Fact]
        public void ReturnApiUrl()
        {
            Assert.Equal("http://api", _baseUrls.Api);
        }

        [Fact]
        public void ReturnIdentityServerUrl()
        {
            Assert.Equal("http://identity", _baseUrls.IdentityServer);
        }

        [Fact]
        public void ReturnGraphqlPlaygroundJsClientUrl()
        {
            Assert.Equal("http://graphqlPlayground", _baseUrls.GraphqlPlaygroundJsClient);
        }

        [Fact]
        public void ReturnReactClientUrl()
        {
            Assert.Equal("http://react", _baseUrls.ReactClient);
        }

        [Fact]
        public void ReturnReactClientLocalUrl()
        {
            Assert.Equal("http://reactLocal", _baseUrls.ReactClientLocal);
        }

        [Fact]
        public void ReturnCorsUrls()
        {
            Assert.Collection(_baseUrls.CorsUrls,
                url => Assert.Equal("http://graphqlPlayground", url),
                url => Assert.Equal("http://react", url),
                url => Assert.Equal("http://reactLocal", url)
            );
        }

        [Fact]
        public void ReturnOnlyNonEmptyCorsUrls()
        {
            _configurationMock.Setup(x => x["BaseUrls:GraphqlPlaygroundJsClient"]).Returns("");
            _configurationMock.Setup(x => x["BaseUrls:ReactClientLocal"]).Returns((string)null);

            Assert.Collection(_baseUrls.CorsUrls,
                url => Assert.Equal("http://react", url)
            );
        }
    }
}
