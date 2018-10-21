using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Domain.MultiTenancy
{
    public interface IMustHaveTenant
    {
        int TenantId { get; set; }
        Tenant Tenant { get; set; }
    }
}
