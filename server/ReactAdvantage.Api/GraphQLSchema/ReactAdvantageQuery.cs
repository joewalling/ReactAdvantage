using System;
using System.Collections.Generic;
using GraphQL.Types;
using ReactAdvantage.Data;
using System.Linq;

namespace ReactAdvantage.Api.GraphQLSchema
{
    public class ReactAdvantageQuery : ObjectGraphType
    {
        public ReactAdvantageQuery(ReactAdvantageContext db)
        {
            Name = "Query";

            Field<UserType>(
                "user",
                arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "id" }),
                resolve: context => db.Users.FindAsync(context.GetArgument<string>("id"))
            );

            Field<ListGraphType<UserType>>(
                "users",
                arguments: new QueryArguments(
                    new QueryArgument<ListGraphType<StringGraphType>> { Name = "id" },
                    new QueryArgument<StringGraphType> { Name = "firstname" },
                    new QueryArgument<StringGraphType> { Name = "lastname" },
                    new QueryArgument<StringGraphType> { Name = "username" },
                    new QueryArgument<StringGraphType> { Name = "email" },
                    new QueryArgument<BooleanGraphType> { Name = "isactive" }
                ),
                resolve: context => db.Users
                    .HandleQueryArgument(new ArgumentGetter<List<string>>("id", context), (arg, query) =>
                        query.Where(x => arg.Contains(x.Id)))
                    .HandleQueryArgument(new ArgumentGetter<string>("firstname", context), (arg, query) => 
                        string.IsNullOrEmpty(arg) ? query : query.Where(x => x.FirstName.Contains(arg)))
                    .HandleQueryArgument(new ArgumentGetter<string>("lastname", context), (arg, query) =>
                        string.IsNullOrEmpty(arg) ? query : query.Where(x => x.LastName.Contains(arg)))
                    .HandleQueryArgument(new ArgumentGetter<string>("username", context), (arg, query) =>
                        string.IsNullOrEmpty(arg) ? query : query.Where(x => x.UserName.Contains(arg)))
                    .HandleQueryArgument(new ArgumentGetter<string>("email", context), (arg, query) =>
                        string.IsNullOrEmpty(arg) ? query : query.Where(x => x.Email.Contains(arg)))
                    .HandleQueryArgument(new ArgumentGetter<bool>("isactive", context), (arg, query) =>
                        query.Where(x => x.IsActive == arg))
            );

            Field<ProjectType>(
                "project",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: context => db.Projects.FindAsync(context.GetArgument<int>("id"))
            );

            Field<ListGraphType<ProjectType>>(
                "projects",
                arguments: new QueryArguments(
                    new QueryArgument<ListGraphType<IntGraphType>> { Name = "id" },
                    new QueryArgument<StringGraphType> { Name = "name" }
                ),
                resolve: context => db.Projects
                    .HandleQueryArgument(new ArgumentGetter<List<int?>>("id", context), (arg, query) =>
                        query.Where(x => arg.Contains(x.Id)))
                    .HandleQueryArgument(new ArgumentGetter<string>("name", context), (arg, query) =>
                        string.IsNullOrEmpty(arg) ? query : query.Where(x => x.Name.Contains(arg)))
            );

            Field<TaskType>(
                "task",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: context => db.Tasks.FindAsync(context.GetArgument<int>("id"))
            );

            Field<ListGraphType<TaskType>>(
                "tasks",
                arguments: new QueryArguments(
                    new QueryArgument<ListGraphType<IntGraphType>> { Name = "id" },
                    new QueryArgument<IntGraphType> { Name = "projectid" },
                    new QueryArgument<StringGraphType> { Name = "name" },
                    new QueryArgument<BooleanGraphType> { Name = "iscompleted" },
                    new QueryArgument<DateGraphType> { Name = "duedate" },
                    new QueryArgument<DateGraphType> { Name = "completiondate" }
                ),
                resolve: context => db.Tasks
                    .HandleQueryArgument(new ArgumentGetter<List<int?>>("id", context), (arg, query) =>
                        query.Where(x => arg.Contains(x.Id)))
                    .HandleQueryArgument(new ArgumentGetter<int>("projectid", context), (arg, query) =>
                        query.Where(x => x.ProjectId == arg))
                    .HandleQueryArgument(new ArgumentGetter<string>("name", context), (arg, query) =>
                        string.IsNullOrEmpty(arg) ? query : query.Where(x => x.Name.Contains(arg)))
                    .HandleQueryArgument(new ArgumentGetter<bool>("iscompleted", context), (arg, query) =>
                        query.Where(x => x.Completed == arg))
                    .HandleQueryArgument(new ArgumentGetter<DateTime>("duedate", context), (arg, query) =>
                        query.Where(x => x.DueDate >= arg.Date && x.DueDate < arg.Date.AddDays(1)))
                    .HandleQueryArgument(new ArgumentGetter<DateTime>("completiondate", context), (arg, query) =>
                        query.Where(x => x.CompletionDate >= arg.Date && x.CompletionDate < arg.Date.AddDays(1)))
            );

        }
    }
}