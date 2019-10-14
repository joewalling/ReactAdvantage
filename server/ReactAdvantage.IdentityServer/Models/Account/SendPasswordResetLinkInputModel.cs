using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReactAdvantage.IdentityServer.Models.Account
{
    public class SendPasswordResetLinkInputModel
    {
        [Display(Name = "Tenant")]
        public string TenantName { get; set; }
        [Required]
        public string Username { get; set; }
    }
}
