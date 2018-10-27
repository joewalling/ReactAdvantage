using Microsoft.AspNetCore.Identity;
using ReactAdvantage.Domain.MultiTenancy;

namespace ReactAdvantage.Domain.Models
{
    public class Role : IdentityRole, IMayHaveTenant
    {
        public Role()
        {
        }

        public Role(int? tenantId, string roleName)
            : base(roleName)
        {
            TenantId = tenantId;
        }

        public string DisplayName { get; set; }

        public bool IsStatic { get; set; }

        public int? TenantId { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
