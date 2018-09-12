using GraphQL.Types;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.GraphQLSchema.Types
{
    public class ProjectInputType : InputObjectGraphType<Project>
    {
        public ProjectInputType()
        {
            Name = "ProjectInput";
            Field(x => x.Id, nullable: true);
            Field(x => x.Name, nullable: true);
        }
    }
}
