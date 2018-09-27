using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer4.AccessTokenValidation;
using Microsoft.Extensions.DependencyInjection;
using ReactAdvantage.Api.Extensions;
using Xunit;

namespace ReactAdvantage.Tests.Unit.Api.Extensions
{
    public class ServiceCollectionExtensionsShould
    {
        [Fact]
        public void FindSingletonImplementationInstance()
        {
            //Given
            var service = new IdentityServerAuthenticationOptions { ApiName = "Test" };
            var services = new ServiceCollection();
            services.AddSingleton(new object());
            services.AddSingleton(new List<string>());
            services.AddSingleton(service);
            services.AddSingleton(new HashSet<string>());

            //When
            var foundService = services.FindSingletonImplementationInstance<IdentityServerAuthenticationOptions>();

            //Then
            Assert.NotNull(foundService);
            Assert.Same(service, foundService);
        }

        [Fact]
        public void FindNoSingletonImplementationInstance()
        {
            //Given
            //var service = new IdentityServerAuthenticationOptions { ApiName = "Test" };
            var services = new ServiceCollection();
            services.AddSingleton(new object());
            services.AddSingleton(new List<string>());
            //services.AddSingleton(service);
            services.AddSingleton(new HashSet<string>());

            //When
            var foundService = services.FindSingletonImplementationInstance<IdentityServerAuthenticationOptions>();

            //Then
            Assert.Null(foundService);
        }
    }
}
