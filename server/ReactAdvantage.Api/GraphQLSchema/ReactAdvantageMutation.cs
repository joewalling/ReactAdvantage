using GraphQL.Types;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ReactAdvantage.Api.GraphQLSchema.Models;
using ReactAdvantage.Api.GraphQLSchema.Types;
using ReactAdvantage.Data;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.GraphQLSchema
{
    public class ReactAdvantageMutation : ObjectGraphType
    {
        public ReactAdvantageMutation(
            ReactAdvantageContext db,
            UserManager<User> userManager
            )
        {
            Field<UserType>(
                "addUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }
                ),
                resolve: context =>
                {
                    var userInput = context.GetArgument<UserInput>("user");
                    userInput.Id = null;
                    if (string.IsNullOrEmpty(userInput.Password))
                    {
                        userManager.CreateAsync(userInput).GetAwaiter().GetResult().ThrowOnError();
                    }
                    else
                    {
                        userManager.CreateAsync(userInput, userInput.Password).GetAwaiter().GetResult().ThrowOnError();
                    }
                    return userInput;
                });

            Field<UserType>(
                "editUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }
                ),
                resolve: context =>
                {
                    var userInput = context.GetArgument<UserInput>("user");
                    var user = userManager.FindByIdAsync(userInput.Id).GetAwaiter().GetResult();
                    user.UpdateValuesFrom(userInput);
                    userManager.UpdateAsync(user).GetAwaiter().GetResult().ThrowOnError();

                    if (!string.IsNullOrEmpty(userInput.Password))
                    {
                        userManager.RemovePasswordAsync(user).GetAwaiter().GetResult().ThrowOnError();
                        userManager.AddPasswordAsync(user, userInput.Password).GetAwaiter().GetResult().ThrowOnError();
                    }

                    return userInput;
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
