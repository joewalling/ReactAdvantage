using System.Collections.Generic;
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

        public bool IsStatic { get; set; }

        public int? TenantId { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public void UpdateValuesFrom(Role role)
        {
            Name = role.Name;
        }
    }
}
