using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReactAdvantage.IdentityServer.Models.Account
{
    public class RegisterInputModel
    {
        [Required]
        [Display(Name = "Tenant")]
        public string TenantName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string ReturnUrl { get; set; }
    }
}
