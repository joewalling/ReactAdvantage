using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactAdvantage.Api.Services
{
    public interface ITenantProvider
    {
        int? GetTenantId();
    }
}
