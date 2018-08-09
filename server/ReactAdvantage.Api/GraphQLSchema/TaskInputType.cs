using GraphQL.Types;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.GraphQLSchema
{
    public class TaskInputType : InputObjectGraphType<Task>
    {
        public TaskInputType()
        {
            Name = "TaskInput";
            Field(x => x.Id, nullable: true);
            Field(x => x.ProjectId, nullable: false);
            Field(x => x.Name, nullable: true);
            Field(x => x.Description, nullable: true);
            Field(x => x.DueDate, nullable: true);
            Field(x => x.Completed, nullable: true);
            Field(x => x.CompletionDate, nullable: true);
        }
    }
}
