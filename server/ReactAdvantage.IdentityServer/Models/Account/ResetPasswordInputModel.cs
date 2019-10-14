using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReactAdvantage.IdentityServer.Models.Account
{
    public class ResetPasswordInputModel : ResetPasswordViewModel
    {
        [Required]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
    }
}
