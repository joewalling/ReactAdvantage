using GraphQL.Types;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.GraphQLSchema.Types
{
    public class ProjectType : ObjectGraphType<Project>
    {
        public ProjectType()
        {
            Field(c => c.Id, nullable: false);
            Field(c => c.Name, nullable: true);
        }
    }
}
