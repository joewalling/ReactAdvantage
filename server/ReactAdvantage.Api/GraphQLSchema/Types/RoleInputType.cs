using GraphQL.Types;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.GraphQLSchema.Types
{
    public class RoleInputType : InputObjectGraphType<Role>
    {
        public RoleInputType()
        {
            Name = "RoleInput";
            Field(x => x.Id, nullable: true);
            Field(x => x.Name, nullable: true);
        }
    }
}
