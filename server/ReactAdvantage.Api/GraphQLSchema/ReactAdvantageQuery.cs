using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using ReactAdvantage.Application.Services;
using ReactAdvantage.Data;
using ReactAdvantage.Domain.Models;
using System.Linq;

namespace ReactAdvantage.Api.GraphQLSchema
{
    public class ReactAdvantageQuery : ObjectGraphType
    {
        public ReactAdvantageQuery(ReactAdvantageContext db )
        {
            var projects = new ProjectService();


            Name = "Query";

            Field<UserType>(
                "userbyid",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: context => db.Users.FindAsync(context.GetArgument<int>("id"))
            );

            Field<ListGraphType<UserType>>(
                "allusers",
                resolve: context => db.Users
            );

            // TODO: Need to create a composable query that can include any of Id, FirstName, LastName,  
            // Email,Name to query the user. Note that some of these arguments may be null and query needs 
            // to handle that in a performant manner.
            //Field<ListGraphType<UserType>>(
            //    "users",
            //    arguments: new QueryArguments{
            //        new QueryArgument<IntGraphType> { Name = "id" }
            //    },
            //    resolve: context => db.Users.Where(u => u.Id == context.GetArgument<int>("id"))
            //);

            Field<ListGraphType<ProjectType>>(
                "allprojects",
                resolve: context => db.Projects.ToListAsync()
            );

            Field<ProjectType>(
                  "projectbyid",
                  arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                  resolve: context => db.Projects.FindAsync(context.GetArgument<int>("id"))
              );
            Field<ProjectType>(
                "project",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: context => projects.GetProjectByIdAsync(context.GetArgument<int>("id"))
            );

            // TODO: Need to create a composable query that can include any of Id, FirstName, LastName,  
            // Email,Name to query the user. Note that some of these arguments may be null and query needs 
            // to handle that in a performant manner.
            //Field<ListGraphType<ProjectType>>(
            //    "users",
            //    arguments: new QueryArguments{
            //        new QueryArgument<IntGraphType> { Name = "id" }
            //    },
            //    resolve: context => db.Projects.Where(u => u.Id == context.GetArgument<int>("id"))
            //);



            //Field<TaskType>(
            //    "task",
            //    resolve: context => new Task { Id = 1, Name = "Task 1" }
            //);

            //Field<TasksQuery>(
            //    "tasks",
            //    resolve: context => new TasksQuery(new TaskService())
            //);

        }
    }
}