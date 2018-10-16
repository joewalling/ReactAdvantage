namespace ReactAdvantage.Domain.MultiTenancy
{
    public interface IMustHaveTenant
    {
        int TenantId { get; set; }
    }
}
