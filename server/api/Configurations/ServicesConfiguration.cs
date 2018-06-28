using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ReactAdvantage.API.Services;


namespace ReactAdvantage.API.Configurations
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProjectService, ProjectService>();
          services.AddScoped<ITaskService, TaskService>();
       return services;
        }

        public static IServiceCollection ConfigureSupervisor(this IServiceCollection services)
        {
           // services.AddScoped<IChinookSupervisor, ChinookSupervisor>();

            return services;
        }
        
        public static IServiceCollection AddMiddleware(this IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            return services;
        }

        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            var corsBuilder = new Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin();
            corsBuilder.AllowCredentials();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", corsBuilder.Build());
            });

            return services;
        }
    }
}