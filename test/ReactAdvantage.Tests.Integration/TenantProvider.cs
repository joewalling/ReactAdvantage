using ReactAdvantage.Domain.Services;

namespace ReactAdvantage.Tests.Integration
{
    class FakeTenantProvider : ITenantProvider
    {
        public int? GetTenantId()
        {
            return 1;
        }
    }
}
