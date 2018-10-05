using GraphQL.Types;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.GraphQLSchema.Types
{
    public class TenantInputType : InputObjectGraphType<Tenant>
    {
        public TenantInputType()
        {
            Name = "TenantInput";
            Field(x => x.Id, nullable: true);
            Field(x => x.Name, nullable: false);
        }
    }
}
