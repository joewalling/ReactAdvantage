using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using ReactAdvantage.Domain.MultiTenancy;

namespace ReactAdvantage.Domain.Models
{
    public class User : IdentityUser, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsActive { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public void UpdateValuesFrom(User other)
        {
            //only update the editable fields
            //TenantId = other.TenantId;
            FirstName = other.FirstName;
            LastName = other.LastName;
            UserName = other.UserName;
            Email = other.Email;
            IsActive = other.IsActive;
        }
    }
}
