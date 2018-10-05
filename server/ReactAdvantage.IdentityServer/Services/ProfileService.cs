using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using ReactAdvantage.Domain.Models;
using System.Security.Claims;
using IdentityServer4.Extensions;
using ClaimTypes = ReactAdvantage.Domain.Configuration.ClaimTypes;
using Task = System.Threading.Tasks.Task;

namespace ReactAdvantage.IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;

        public ProfileService(
            UserManager<User> userManager,
            IUserClaimsPrincipalFactory<User> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);
            context.AddRequestedClaims(principal.Claims);

            context.AddRequestedClaims(new[]
            {
                new Claim(ClaimTypes.TenantId, user.TenantId?.ToString())
            });
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user?.IsActive == true;
        }
    }
}
