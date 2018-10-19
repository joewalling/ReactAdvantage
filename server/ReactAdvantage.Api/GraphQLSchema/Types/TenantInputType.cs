using GraphQL.Types;
using ReactAdvantage.Api.GraphQLSchema.Models;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.GraphQLSchema.Types
{
    public class TenantInputType : InputObjectGraphType<TenantInput>
    {
        public TenantInputType()
        {
            Name = "TenantInput";
            Field(x => x.Id, nullable: true);
            Field(x => x.Name, nullable: false);

            Field(x => x.AdminUser, nullable: true);
        }
    }
}
