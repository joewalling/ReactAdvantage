using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ReactAdvantage.Api.Extensions
{
    public static class IdentityExtensions
    {
        public static IdentityBuilder AddIdentityCore<TUser, TRole, TContext>(this IServiceCollection services)
            where TUser : class
            where TRole : class
            where TContext : DbContext
        {
            var builder = services.AddIdentityCore<TUser>(opt =>
                {
                    //opt.Password.RequireDigit = true;
                    //opt.Password.RequiredLength = 8;
                    //opt.Password.RequireNonAlphanumeric = false;
                    //opt.Password.RequireUppercase = true;
                    //opt.Password.RequireLowercase = true;
                }
            );
            builder = new IdentityBuilder(builder.UserType, typeof(TRole), builder.Services);
            builder.AddEntityFrameworkStores<TContext>();
            
            builder.AddRoleValidator<RoleValidator<TRole>>();
            builder.AddRoleManager<RoleManager<TRole>>();

            return builder;
        }
    }
}
