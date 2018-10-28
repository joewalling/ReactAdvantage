namespace ReactAdvantage.Data
{
    public interface IDbInitializer
    {
        void Initialize();
        void SeedTenantRoles(int tenantId);
    }
}
