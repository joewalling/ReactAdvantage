using System.Collections.Generic;
using System.Linq;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Identity;
using ReactAdvantage.Api.GraphQLSchema.Models;
using ReactAdvantage.Api.GraphQLSchema.Types;
using ReactAdvantage.Data;
using ReactAdvantage.Domain.Configuration;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.GraphQLSchema
{
    public class ReactAdvantageMutation : ObjectGraphType
    {
        private readonly ReactAdvantageContext _db;
        private readonly UserManager<User> _userManager;
        
        public ReactAdvantageMutation(
            ReactAdvantageContext db,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IDbInitializer dbInitializer
            )
        {
            _db = db;
            _userManager = userManager;

            Field<TenantType>(
                "addTenant",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<TenantInputType>> { Name = "tenant" },
                    new QueryArgument<NonNullGraphType<UserInputType>> { Name = "adminUser" }
                ),
                resolve: context =>
                {
                    context.GetUserContext().EnsureIsInRole(RoleNames.HostAdministrator);

                    var tenantInput = context.GetArgument<Tenant>("tenant");
                    //tenantInput.Id = 0;
                    var tenant = new Tenant();
                    tenant.UpdateValuesFrom(tenantInput);
                    db.Add(tenant);
                    db.SaveChanges();

                    dbInitializer.SeedTenantRoles(tenant.Id);

                    using (_db.SetTenantFilterValue(tenant.Id))
                    {
                        var adminUser = context.GetArgument<UserInput>("adminUser");
                        adminUser.UserName = "admin";
                        adminUser.TenantId = tenant.Id;
                        adminUser.IsActive = true;
                        adminUser.Roles = adminUser.Roles ?? new List<string>();
                        adminUser.Roles.Add(RoleNames.Administrator);
                        AddUser(adminUser);

                        _userManager.AddToRoleAsync(adminUser, RoleNames.Administrator).GetAwaiter().GetResult();
                    }

                    return tenant;
                });

            Field<TenantType>(
                "editTenant",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<TenantInputType>> { Name = "tenant" }
                ),
                resolve: context =>
                {
                    context.GetUserContext().EnsureIsInRole(RoleNames.HostAdministrator);

                    var tenantInput = context.GetArgument<Tenant>("tenant");
                    var entity = db.Tenants.Find(tenantInput.Id);
                    entity.UpdateValuesFrom(tenantInput);
                    db.SaveChanges();
                    return tenantInput;
                });

            Field<UserType>(
                "addUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }
                ),
                resolve: context =>
                {
                    context.GetUserContext().EnsureIsInEitherRole(RoleNames.HostAdministrator, RoleNames.Administrator);

                    var userInput = context.GetArgument<UserInput>("user");

                    userInput.SetTenantIdOrThrow(context);

                    var userDto = AddUser(userInput);
                    
                    return userDto;
                });

            Field<UserType>(
                "editUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }
                ),
                resolve: context =>
                {
                    var userInput = context.GetArgument<UserInput>("user");

                    var userContext = context.GetUserContext();
                    var isAdmin = userContext.IsInRole(RoleNames.HostAdministrator) 
                                 || userContext.IsInRole(RoleNames.Administrator);
                    var isEditingSelf = userContext.Id == userInput.Id;
                    if (!isAdmin && !isEditingSelf)
                    {
                        throw new ExecutionError($"Unauthorized. You have to be a member of {RoleNames.HostAdministrator}" 
                                                 + $" or {RoleNames.Administrator} role to be able to edit any user," 
                                                 + $" otherwise you can only edit your own user (id: {userContext.Id}).");
                    }

                    if (!isAdmin && userInput.Roles != null)
                    {
                        throw new ExecutionError($"Unauthorized. You have to be a member of {RoleNames.HostAdministrator}" 
                                                + $" or {RoleNames.Administrator} role to be able to change user roles.");
                    }

                    var userDto = EditUser(userInput);

                    return userDto;
                });

            Field<RoleType>(
                "addRole",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<RoleInputType>> { Name = "role" }
                ),
                resolve: context =>
                {
                    context.GetUserContext().EnsureIsInEitherRole(RoleNames.HostAdministrator, RoleNames.Administrator);

                    var role = context.GetArgument<Role>("role");

                    role.Id = null;
                    role.IsStatic = false;
                    role.SetTenantIdOrThrow(context);
                    roleManager.CreateAsync(role).GetAwaiter().GetResult();

                    return role;
                });

            Field<RoleType>(
                "editRole",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<RoleInputType>> { Name = "role" }
                ),
                resolve: context =>
                {
                    var roleInput = context.GetArgument<Role>("role");

                    context.GetUserContext().EnsureIsInEitherRole(RoleNames.HostAdministrator, RoleNames.Administrator);
                    
                    var role = roleManager.FindByIdAsync(roleInput.Id).GetAwaiter().GetResult();

                    if (role.IsStatic)
                    {
                        throw new ExecutionError("You can't edit static roles");
                    }

                    role.UpdateValuesFrom(roleInput);
                    roleManager.UpdateAsync(role).GetAwaiter().GetResult().ThrowOnError();

                    return role;
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
                    project.SetTenantIdOrThrow(context);
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
                    entity.UpdateValuesFrom(project);
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
                    task.SetTenantIdOrThrow(context);
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
                    entity.UpdateValuesFrom(task);
                    db.SaveChanges();
                    return task;
                });
        }

        private UserDto AddUser(UserInput userInput)
        {
            userInput.Id = null;

            if (string.IsNullOrEmpty(userInput.Password))
            {
                _userManager.CreateAsync(userInput).GetAwaiter().GetResult().ThrowOnError();
            }
            else
            {
                _userManager.CreateAsync(userInput, userInput.Password).GetAwaiter().GetResult().ThrowOnError();
            }

            if (userInput.Roles != null)
            {
                foreach (var roleName in userInput.Roles.Distinct())
                {
                    _userManager.AddToRoleAsync(userInput, roleName).GetAwaiter().GetResult();
                }
            }

            var userDto = new UserDto(userInput)
            {
                Roles = _userManager.GetRolesAsync(userInput).GetAwaiter().GetResult().ToArray()
            };

            return userDto;
        }

        private UserDto EditUser(UserInput userInput)
        {
            var user = _userManager.FindByIdAsync(userInput.Id).GetAwaiter().GetResult();

            if (!string.IsNullOrEmpty(userInput.Password))
            {
                //validate the password before editing the user so we can leave the user unchanged if the validation fails
                foreach (var validator in _userManager.PasswordValidators)
                {
                    validator.ValidateAsync(_userManager, user, userInput.Password).GetAwaiter().GetResult()
                        .ThrowOnError();
                }
            }

            user.UpdateValuesFrom(userInput);
            _userManager.UpdateAsync(user).GetAwaiter().GetResult().ThrowOnError();

            if (!string.IsNullOrEmpty(userInput.Password))
            {
                _userManager.RemovePasswordAsync(user).GetAwaiter().GetResult().ThrowOnError();
                _userManager.AddPasswordAsync(user, userInput.Password).GetAwaiter().GetResult().ThrowOnError();
                //the below doesn't work without adding IUserTwoFactorTokenProvider
                //var resetToken = _userManager.GeneratePasswordResetTokenAsync(user).GetAwaiter().GetResult();
                //_userManager.ResetPasswordAsync(user, resetToken, userInput.Password).GetAwaiter().GetResult().ThrowOnError();
            }

            if (userInput.Roles != null)
            {
                var existingRoles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();
                foreach (var roleToAdd in userInput.Roles.Except(existingRoles).Distinct())
                {
                    _userManager.AddToRoleAsync(user, roleToAdd).GetAwaiter().GetResult();
                }

                foreach (var roleToRemove in existingRoles.Except(userInput.Roles).Distinct())
                {
                    _userManager.RemoveFromRoleAsync(user, roleToRemove).GetAwaiter().GetResult();
                }
            }

            var userDto = new UserDto(user)
            {
                Roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().ToArray()
            };

            return userDto;
        }
    }
}
