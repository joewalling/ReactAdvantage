using GraphQL.Types;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.GraphQLSchema.Types
{
    public class TenantType : ObjectGraphType<Tenant>
    {
        public TenantType()
        {
            Field(x => x.Id, nullable: false);
            Field(x => x.Name, nullable: false);
        }
    }
}
