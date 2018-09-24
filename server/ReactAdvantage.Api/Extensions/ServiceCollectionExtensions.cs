using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace ReactAdvantage.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static T FindSingletonImplementationInstance<T>(this IServiceCollection services) 
            where T : class
        {
            return services.FirstOrDefault(x => x.ServiceType == typeof(T))?.ImplementationInstance as T;
        }
    }
}
