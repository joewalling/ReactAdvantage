using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace ReactAdvantage.Api.GraphQLSchema
{
    public static class GraphQLSetup
    {
        public static IServiceCollection AddGraphqlServices(this IServiceCollection services)
        {
            services.AddTransient<IDocumentExecuter, DocumentExecuter>();
            services.AddTransient<ReactAdvantageQuery>();
            services.AddTransient<ReactAdvantageMutation>();
            services.AddTransient<TaskType>();
            services.AddTransient<TaskInputType>();
            services.AddTransient<UserType>();
            services.AddTransient<UserInputType>();
            services.AddTransient<ProjectType>();
            services.AddTransient<ProjectInputType>();

            var sp = services.BuildServiceProvider();
            services.AddTransient<ISchema>(x => new ReactAdvantageSchema(new FuncDependencyResolver(type => sp.GetService(type))));

            return services;
        }
    }
}
