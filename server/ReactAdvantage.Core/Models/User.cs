using Microsoft.AspNetCore.Identity;

namespace ReactAdvantage.Domain.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsActive { get; set; }

        public void UpdateValuesFrom(User other)
        {
            FirstName = other.FirstName;
            LastName = other.LastName;
            UserName = other.UserName;
            Email = other.Email;
            IsActive = other.IsActive;
        }
    }
}
