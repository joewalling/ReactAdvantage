using System.Linq;
using System.Security.Claims;
using ClaimTypes = ReactAdvantage.Domain.Configuration.ClaimTypes;

namespace ReactAdvantage.Domain.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            return user?.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            //return user?.FindFirst("sub")?.Value;
        }

        public static int? GetTenantId(this ClaimsPrincipal user)
        {
            var tenantIdString = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.TenantId)?.Value;
            
            if (int.TryParse(tenantIdString, out var tenantId))
            {
                return tenantId;
            }

            return null;
        }
    }
}
