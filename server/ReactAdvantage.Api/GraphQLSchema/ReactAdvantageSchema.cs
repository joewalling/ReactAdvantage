using GraphQL;
using Microsoft.EntityFrameworkCore;
using ReactAdvantage.Data;

namespace ReactAdvantage.Api.GraphQLSchema
{
    public class ReactAdvantageSchema : GraphQL.Types.Schema
    {

        public ReactAdvantageSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<ReactAdvantageQuery>();
            //Mutation = mutation;
            //Subscription = subscription;
        }
    }
}
