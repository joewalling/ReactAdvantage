namespace ReactAdvantage.Domain.MultiTenancy
{
    public interface IMayHaveTenant
    {
        int? TenantId { get; set; }
    }
}
