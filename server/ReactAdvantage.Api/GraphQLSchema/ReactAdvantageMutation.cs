using GraphQL.Types;
using ReactAdvantage.Data;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.GraphQLSchema
{
    public class ReactAdvantageMutation : ObjectGraphType
    {
        public ReactAdvantageMutation(
            ReactAdvantageContext db
            //UserManager<User> userManager //todo
            )
        {
            Field<UserType>(
                "addUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }
                ),
                resolve: context =>
                {
                    var user = context.GetArgument<User>("user");
                    user.Id = null;
                    db.Add(user);
                    db.SaveChanges();
                    return user;
                });

            Field<UserType>(
                "editUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }
                ),
                resolve: context =>
                {
                    var user = context.GetArgument<User>("user");
                    var entity = db.Users.Find(user.Id);
                    db.Entry(entity).CurrentValues.SetValues(user);
                    db.SaveChanges();
                    return user;
                });

            Field<ProjectType>(
                "addProject",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ProjectInputType>> { Name = "project" }
                ),
                resolve: context =>
                {
                    var project = context.GetArgument<Project>("project");
                    project.Id = 0;
                    db.Add(project);
                    db.SaveChanges();
                    return project;
                });

            Field<ProjectType>(
                "editProject",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ProjectInputType>> { Name = "project" }
                ),
                resolve: context =>
                {
                    var project = context.GetArgument<Project>("project");
                    var entity = db.Projects.Find(project.Id);
                    db.Entry(entity).CurrentValues.SetValues(project);
                    db.SaveChanges();
                    return project;
                });

            Field<TaskType>(
                "addTask",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<TaskInputType>> { Name = "task" }
                ),
                resolve: context =>
                {
                    var task = context.GetArgument<Task>("task");
                    task.Id = 0;
                    db.Add(task);
                    db.SaveChanges();
                    return task;
                });

            Field<TaskType>(
                "editTask",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<TaskInputType>> { Name = "task" }
                ),
                resolve: context =>
                {
                    var task = context.GetArgument<Task>("task");
                    var entity = db.Tasks.Find(task.Id);
                    db.Entry(entity).CurrentValues.SetValues(task);
                    db.SaveChanges();
                    return task;
                });
        }
    }
}
