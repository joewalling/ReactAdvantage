using Microsoft.AspNetCore.Http;
using ReactAdvantage.Domain.Extensions;
using ReactAdvantage.Domain.Services;

namespace ReactAdvantage.IdentityServer.Services
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetTenantId()
        {
            return _httpContextAccessor.HttpContext?.User?.GetTenantId();
        }
    }
}
