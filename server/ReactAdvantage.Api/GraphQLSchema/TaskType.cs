using GraphQL.Types;
using ReactAdvantage.Data;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.GraphQLSchema
{
    public class TaskType : ObjectGraphType<Task>
    {
        public TaskType(ReactAdvantageContext db)
        {
            Field(c => c.Id);
            Field(c => c.Name, nullable: true);
            Field(o => o.Description, nullable: true);
            Field(o => o.DueDate, nullable: true);
            Field(o => o.Completed, nullable: false);
            Field(o => o.CompletionDate, nullable: true);
            Field<ProjectType>("project",
                resolve: context => db.Projects.FindAsync(context.Source.ProjectId));
        }
    }
}
