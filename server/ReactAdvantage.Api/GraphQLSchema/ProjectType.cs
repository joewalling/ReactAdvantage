using GraphQL.Types;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.GraphQLSchema
{
    public class ProjectType : ObjectGraphType<Project>
    {
        public ProjectType()
        {
            Field(c => c.Id);
            Field(c => c.Name);
        }
    }
}
