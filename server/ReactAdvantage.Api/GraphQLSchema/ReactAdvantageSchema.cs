using GraphQL;

namespace ReactAdvantage.Api.GraphQLSchema
{
    public class ReactAdvantageSchema : GraphQL.Types.Schema
    {

        public ReactAdvantageSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<ReactAdvantageQuery>();
            Mutation = resolver.Resolve<ReactAdvantageMutation>();
            //Subscription = subscription;
        }
    }
}
