using GraphQL.Types;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.GraphQLSchema.Types
{
    public class RoleType : ObjectGraphType<Role>
    {
        public RoleType()
        {
            Field(c => c.Id, nullable: false);
            Field(c => c.Name, nullable: true);
            Field(c => c.IsStatic, nullable: false);
        }
    }
}
