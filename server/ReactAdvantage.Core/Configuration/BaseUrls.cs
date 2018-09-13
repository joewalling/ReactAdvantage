using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ReactAdvantage.Domain.Configuration
{
    public class BaseUrls
    {
        private readonly IConfiguration _configuration;

        public BaseUrls(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Api => _configuration["BaseUrls:Api"];

        public string IdentityServer => _configuration["BaseUrls:IdentityServer"];

        public string GraphqlPlaygroundJsClient => _configuration["BaseUrls:GraphqlPlaygroundJsClient"];

    }

    public static class BaseUrlExtensions
    {
        public static BaseUrls GetBaseUrls(this IConfiguration configuration)
        {
            return new BaseUrls(configuration);
        }
    }
}
