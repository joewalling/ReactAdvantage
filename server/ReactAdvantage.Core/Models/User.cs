using Microsoft.AspNetCore.Identity;

namespace ReactAdvantage.Domain.Models
{
    public class User : IdentityUser
    {
        public int? TenantId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsActive { get; set; }

        public virtual Tenant Tenant { get; set; }

        public void UpdateValuesFrom(User other)
        {
            //TenantId = other.TenantId; //only update the editable fields
            FirstName = other.FirstName;
            LastName = other.LastName;
            UserName = other.UserName;
            Email = other.Email;
            IsActive = other.IsActive;
        }
    }
}
