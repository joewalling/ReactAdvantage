using System;
using GraphQL;

namespace ReactAdvantage.Api.GraphQLSchema
{
    public class ReactAdvantageSchema : GraphQL.Types.Schema
    {
        public ReactAdvantageSchema(IServiceProvider serviceProvider)
            : base(new FuncDependencyResolver(serviceProvider.GetService))
        {
            Query = DependencyResolver.Resolve<ReactAdvantageQuery>();
            Mutation = DependencyResolver.Resolve<ReactAdvantageMutation>();
            //Subscription = subscription;
        }
    }
}
