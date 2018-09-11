using Microsoft.AspNetCore.Identity;

namespace ReactAdvantage.Domain.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsActive { get; set; }
    }
}
