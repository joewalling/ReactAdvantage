using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactAdvantage.IdentityServer.Models.Account
{
    public class ResetPasswordViewModel
    {
        public string UserId { get; set; }
        public string Code { get; set; }

        public ResetPasswordInputModel ToInputModel()
        {
            return new ResetPasswordInputModel
            {
                Code = Code,
                UserId = UserId
            };
        }
    }
}
