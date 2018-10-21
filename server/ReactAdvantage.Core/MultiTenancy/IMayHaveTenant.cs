using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Domain.MultiTenancy
{
    public interface IMayHaveTenant
    {
        int? TenantId { get; set; }
        Tenant Tenant { get; set; }
    }
}
