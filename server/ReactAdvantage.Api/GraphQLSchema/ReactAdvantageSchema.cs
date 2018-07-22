using Microsoft.EntityFrameworkCore;
using ReactAdvantage.Data;

namespace ReactAdvantage.Api.GraphQLSchema
{
    public class ReactAdvantageSchema : GraphQL.Types.Schema
    {

        public ReactAdvantageSchema(ReactAdvantageContext db)
        {

            Query = new ReactAdvantageQuery(db);
            //Mutation = mutation;
            //Subscription = subscription;
            //DependencyResolver = resolver;
        }
    }
}
