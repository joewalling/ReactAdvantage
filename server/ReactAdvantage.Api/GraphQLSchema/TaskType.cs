using GraphQL.Types;
using ReactAdvantage.Application.Services;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.GraphQLSchema
{
    public class TaskType : ObjectGraphType<Task>
    {
        public TaskType(IProjectService projects)
        {
            Field(c => c.Id);
            Field(c => c.Name);
            Field(o => o.Description);
            Field(o => o.DueDate);
            Field(o => o.Completed);
            Field(o => o.CompletionDate);
            Field<ProjectType>("project",
                resolve: context => projects.GetProjectByIdAsync(context.Source.ProjectId));

        }
    }
}
