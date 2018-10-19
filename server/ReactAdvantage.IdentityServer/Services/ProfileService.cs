using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using ReactAdvantage.Domain.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using ReactAdvantage.Data;
using ClaimTypes = ReactAdvantage.Domain.Configuration.ClaimTypes;
using Task = System.Threading.Tasks.Task;

namespace ReactAdvantage.IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ReactAdvantageContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;

        public ProfileService(
            ReactAdvantageContext db,
            UserManager<User> userManager,
            IUserClaimsPrincipalFactory<User> claimsFactory)
        {
            _db = db;
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await GetUserAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);
            context.AddRequestedClaims(principal.Claims);

            context.AddRequestedClaims(new[]
            {
                new Claim(ClaimTypes.TenantId, user.TenantId?.ToString() ?? "")
            });
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await GetUserAsync(sub);
            context.IsActive = user?.IsActive == true;
        }

        private async Task<User> GetUserAsync(string subjectId)
        {
            using (_db.DisableTenantFilter())
            {
                return await _userManager.FindByIdAsync(subjectId);
            }
        }
    }
}
