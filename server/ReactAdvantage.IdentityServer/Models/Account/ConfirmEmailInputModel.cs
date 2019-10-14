using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactAdvantage.IdentityServer.Models.Account
{
    public class ConfirmEmailInputModel
    {
        public string UserId { get; set; }
        public string Code { get; set; }
    }
}
