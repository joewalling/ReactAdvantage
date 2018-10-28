using ReactAdvantage.Api.GraphQLSchema;
using ReactAdvantage.Domain.Models;
using System;
using System.Linq;
using GraphQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ReactAdvantage.Data;
using ReactAdvantage.Domain.Configuration;
using Xunit;
using Threading = System.Threading.Tasks;

namespace ReactAdvantage.Tests.Unit.Api.Graphql
{
    public class ReactAdvantageMutationShould : GraphqlTestBase
    {
        [Fact]
        public async void AddTenantShouldPassForHostAdmin()
        {
            // Given
            UserContextMock.Setup(x => x.IsInRole(RoleNames.HostAdministrator)).Returns(true);

            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = $@"
                    mutation 
                    {{ 
                        addTenant(
                            tenant: {{
                                name: ""TestTenant""
                            }},
                            adminUser: {{ 
                                userName: ""admin""
                                email: ""admin@test.com""
                                isActive: true
                                password: ""Test123$""
                            }}
                        )
                        {{ 
                            id
                            name
                        }}
                    }}"
            });

            // Then
            int tenantId = 0;

            AssertGraphqlResultDictionary(result.Data,
                addTenantResult => AssertPairEqual(addTenantResult,
                    "addTenant", tenant => AssertGraphqlResultDictionary(tenant,
                        field => AssertPairEqual(field, "id", idObject =>
                        {
                            var id = Assert.IsType<int>(idObject);
                            Assert.NotEqual(0, id);
                            tenantId = id;
                        }),
                        field => AssertPairEqual(field, "name", "TestTenant")
                    )
                )
            );

            Assert.NotEqual(0, tenantId);

            using (var db = GetInMemoryDbContext())
            using (db.SetTenantFilterValue(tenantId))
            {
                var tenant = db.Tenants.Find(tenantId);
                Assert.NotNull(tenant);
                Assert.Equal("TestTenant", tenant.Name);

                var adminUser = db.Users.Single();
                Assert.Equal("admin", adminUser.UserName);
                Assert.Equal("admin@test.com", adminUser.Email);
                Assert.True(adminUser.IsActive);

                var adminRole = db.Roles.First(x => x.Name == RoleNames.Administrator);
                Assert.True(adminRole.IsStatic);
                var userRole = db.Roles.First(x => x.Name == RoleNames.User);
                Assert.True(userRole.IsStatic);
            }
        }

        [Fact]
        public async void AddTenantShouldFailForNonHostAdmin()
        {
            // Given
            UserContextMock.Setup(x => x.IsInRole(RoleNames.HostAdministrator)).Returns(false);
            UserContextMock.Setup(x => x.IsInRole(RoleNames.Administrator)).Returns(true);

            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = $@"
                    mutation 
                    {{ 
                        addTenant(
                            tenant: {{
                                name: ""TestTenant""
                            }},
                            adminUser: {{ 
                                userName: ""admin""
                                email: ""admin@test.com""
                                isActive: true
                                password: ""Test123$""
                            }}
                        )
                        {{ 
                            id
                            name
                        }}
                    }}"
            });

            // Then
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            AssertGraphqlResultDictionary(result.Data,
                addUserResult => AssertPairEqual(addUserResult, "addTenant", null)
            );
            Assert.Collection(result.Errors, error =>
            {
                var exception = Assert.IsType<ExecutionError>(error);
                Assert.Equal($"Unauthorized. You have to be a member of {RoleNames.HostAdministrator} role.", exception.Message);
            });
        }

        [Fact]
        public async void AddUserShouldFailForNonAdmin()
        {
            // Given
            UserContextMock.Setup(x => x.IsInRole(RoleNames.HostAdministrator)).Returns(false);
            UserContextMock.Setup(x => x.IsInRole(RoleNames.Administrator)).Returns(false);

            // When
            var result = await ExecuteAddUserQueryAsync();

            // Then
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            AssertGraphqlResultDictionary(result.Data,
                addUserResult => AssertPairEqual(addUserResult, "addUser", null)
            );
            Assert.Collection(result.Errors, error =>
            {
                var exception = Assert.IsType<ExecutionError>(error);
                Assert.Equal("Unauthorized. You have to be a member of either one of these roles: HostAdministrator, Administrator", exception.Message);
            });
        }

        [Fact]
        public async void AddUserShouldPassForHostAdmin()
        {
            // Given
            UserContextMock.Setup(x => x.IsInRole(RoleNames.HostAdministrator)).Returns(true);
            UserContextMock.Setup(x => x.IsInRole(RoleNames.Administrator)).Returns(false);

            // When
            var result = await ExecuteAddUserQueryAsync();

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertAddUserQueryPassed(result);
        }

        [Fact]
        public async void AddUserShouldPassForAdmin()
        {
            // Given
            UserContextMock.Setup(x => x.IsInRole(RoleNames.HostAdministrator)).Returns(false);
            UserContextMock.Setup(x => x.IsInRole(RoleNames.Administrator)).Returns(true);

            // When
            var result = await ExecuteAddUserQueryAsync();

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertAddUserQueryPassed(result);
        }

        [Fact]
        public async void AddUserWithNoPassword()
        {
            // Given
            UserContextMock.Setup(x => x.IsInRole(RoleNames.HostAdministrator)).Returns(false);
            UserContextMock.Setup(x => x.IsInRole(RoleNames.Administrator)).Returns(true);

            // When
            var result = await ExecuteAddUserQueryAsync();

            // Then
            AssertValidGraphqlExecutionResult(result);

            var userId = AssertAddUserQueryPassed(result);

            using (var db = GetInMemoryDbContext())
            {
                var user = db.Users.Find(userId);
                Assert.Null(user.PasswordHash);
                Assert.False(await UserManager.CheckPasswordAsync(user, "Test123$"));
            }
        }

        [Fact]
        public async void AddUserWithPassword()
        {
            // Given
            UserContextMock.Setup(x => x.IsInRole(RoleNames.HostAdministrator)).Returns(true);

            // When
            var result = await ExecuteAddUserQueryAsync(setPassword: true, password: "Test123$");

            // Then
            AssertValidGraphqlExecutionResult(result);

            var userId = AssertAddUserQueryPassed(result);

            using (var db = GetInMemoryDbContext())
            {
                var user = db.Users.Find(userId);
                Assert.True(await UserManager.CheckPasswordAsync(user, "Test123$"));
            }
        }

        private async Threading.Task<ExecutionResult> ExecuteAddUserQueryAsync(bool setPassword = false, string password = null)
        {
            DbInitializer.SeedTenantRoles(1);

            return await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = $@"
                    mutation 
                    {{ 
                        addUser(user: {{ 
                            userName: ""TestUser""
                            firstName: ""Tom""
                            lastName: ""Smith""
                            email: ""test@test.com""
                            isActive: true
                            {(setPassword ? $@"password: ""{password}""" : "")}
                            roles: [
                                ""{RoleNames.User}""
                            ]
                        }})
                        {{ 
                            id
                            userName
                            firstName
                            lastName
                            email
                            isActive
                            roles
                        }}
                    }}"
            });
        }

        private string AssertAddUserQueryPassed(ExecutionResult result)
        {
            string userId = null;

            AssertGraphqlResultDictionary(result.Data,
                addUserResult => AssertPairEqual(addUserResult,
                    "addUser", user => AssertGraphqlResultDictionary(user,
                        field => AssertPairEqual(field, "id", idObject =>
                        {
                            var id = Assert.IsType<string>(idObject);
                            Assert.False(string.IsNullOrEmpty(id));
                            userId = id;
                        }),
                        field => AssertPairEqual(field, "userName", "TestUser"),
                        field => AssertPairEqual(field, "firstName", "Tom"),
                        field => AssertPairEqual(field, "lastName", "Smith"),
                        field => AssertPairEqual(field, "email", "test@test.com"),
                        field => AssertPairEqual(field, "isActive", true),
                        field => AssertPairEqual(field, "roles",
                            roles => AssertGraphqlResultArray<string>(roles,
                                role => Assert.Equal(role, RoleNames.User)
                            )
                        )
                    )
                )
            );

            Assert.NotNull(userId);

            using (var db = GetInMemoryDbContext())
            {
                var user = db.Users.Find(userId);
                Assert.Equal("TestUser", user.UserName);
                Assert.Equal("Tom", user.FirstName);
                Assert.Equal("Smith", user.LastName);
                Assert.Equal("test@test.com", user.Email);
                Assert.True(user.IsActive);
            }

            return userId;
        }

        [Fact]
        public async void EditUserShouldFailForNonSelfNonHostAdminNonAdmin()
        {
            // Given
            UserContextMock.Setup(x => x.IsInRole(RoleNames.HostAdministrator)).Returns(false);
            UserContextMock.Setup(x => x.IsInRole(RoleNames.Administrator)).Returns(false);
            UserContextMock.Setup(x => x.Id).Returns("2");

            await SeedInitialUserForEdit();

            // When
            var result = await ExecuteEditUserQueryAsync();
            
            // Then
            AssertEditUserQueryFailed(result, error =>
            {
                var exception = Assert.IsType<ExecutionError>(error);
                Assert.Equal($"Unauthorized. You have to be a member of {RoleNames.HostAdministrator}"
                             + $" or {RoleNames.Administrator} role to be able to edit any user, otherwise you can only edit"
                             + " your own user (id: 2).", exception.Message);
            });
        }

        [Fact]
        public async void EditUserShouldPassForSelfNonAdmin()
        {
            // Given
            UserContextMock.Setup(x => x.IsInRole(RoleNames.HostAdministrator)).Returns(false);
            UserContextMock.Setup(x => x.IsInRole(RoleNames.Administrator)).Returns(false);
            UserContextMock.Setup(x => x.Id).Returns("1");

            await SeedInitialUserForEdit();

            // When
            var result = await ExecuteEditUserQueryAsync(changeRoles: false);

            // Then
            AssertEditUserQueryPassed(result, expectChangedRoles: false);
        }

        [Fact]
        public async void EditUserRolesShouldFailForSelfNonAdmin()
        {
            // Given
            UserContextMock.Setup(x => x.IsInRole(RoleNames.HostAdministrator)).Returns(false);
            UserContextMock.Setup(x => x.IsInRole(RoleNames.Administrator)).Returns(false);
            UserContextMock.Setup(x => x.Id).Returns("1");

            await SeedInitialUserForEdit();

            // When
            var result = await ExecuteEditUserQueryAsync(changeRoles: true);

            // Then

            AssertEditUserQueryFailed(result, error =>
            {
                var exception = Assert.IsType<ExecutionError>(error);
                Assert.Equal($"Unauthorized. You have to be a member of {RoleNames.HostAdministrator}"
                             + $" or {RoleNames.Administrator} role to be able to change user roles.", exception.Message);
            });
        }

        [Fact]
        public async void EditUserShouldPassForNonSelfAndHostAdmin()
        {
            // Given
            UserContextMock.Setup(x => x.IsInRole(RoleNames.HostAdministrator)).Returns(true);
            UserContextMock.Setup(x => x.IsInRole(RoleNames.Administrator)).Returns(false);
            UserContextMock.Setup(x => x.Id).Returns("2");

            await SeedInitialUserForEdit();

            // When
            var result = await ExecuteEditUserQueryAsync();

            // Then
            AssertEditUserQueryPassed(result);
        }

        [Fact]
        public async void EditUserShouldPassForNonSelfAndAdmin()
        {
            // Given
            UserContextMock.Setup(x => x.IsInRole(RoleNames.HostAdministrator)).Returns(false);
            UserContextMock.Setup(x => x.IsInRole(RoleNames.Administrator)).Returns(true);
            UserContextMock.Setup(x => x.Id).Returns("2");

            await SeedInitialUserForEdit();

            // When
            var result = await ExecuteEditUserQueryAsync();

            // Then
            AssertEditUserQueryPassed(result);
        }

        [Fact]
        public async void EditUserShouldPassForSelfAndHostAdmin()
        {
            // Given
            UserContextMock.Setup(x => x.IsInRole(RoleNames.HostAdministrator)).Returns(true);
            UserContextMock.Setup(x => x.IsInRole(RoleNames.Administrator)).Returns(true);
            UserContextMock.Setup(x => x.Id).Returns("1");

            await SeedInitialUserForEdit();

            // When
            var result = await ExecuteEditUserQueryAsync();

            // Then
            AssertEditUserQueryPassed(result);
        }

        [Fact]
        public async void EditUserWithoutChangingThePassword()
        {
            // Given
            UserContextMock.Setup(x => x.IsInRole(RoleNames.HostAdministrator)).Returns(true);
            UserContextMock.Setup(x => x.Id).Returns("1");

            var seededUser = await SeedInitialUserForEdit();
            await UserManager.AddPasswordAsync(seededUser, "Test123$");

            string oldPasswordHash;
            using (var db = GetInMemoryDbContext())
            {
                var user = db.Users.Find("1");
                oldPasswordHash = user.PasswordHash;
            }
            
            // When
            var result = await ExecuteEditUserQueryAsync(changePassword: false);

            // Then
            AssertEditUserQueryPassed(result);

            string newPasswordHash;
            using (var db = GetInMemoryDbContext())
            {
                var user = db.Users.Find("1");
                newPasswordHash = user.PasswordHash;
            }

            Assert.Equal(oldPasswordHash, newPasswordHash);
            Assert.True(await UserManager.CheckPasswordAsync(seededUser, "Test123$"));
        }

        [Fact]
        public async void EditUserWithChangingThePassword()
        {
            // Given
            UserContextMock.Setup(x => x.IsInRole(RoleNames.HostAdministrator)).Returns(true);
            UserContextMock.Setup(x => x.Id).Returns("1");

            var seededUser = await SeedInitialUserForEdit();
            await UserManager.AddPasswordAsync(seededUser, "Test123$");

            string oldPasswordHash;
            using (var db = GetInMemoryDbContext())
            {
                var user = db.Users.Find("1");
                oldPasswordHash = user.PasswordHash;
            }

            // When
            var result = await ExecuteEditUserQueryAsync(changePassword: true);

            // Then
            AssertEditUserQueryPassed(result);

            string newPasswordHash;
            using (var db = GetInMemoryDbContext())
            {
                var user = db.Users.Find("1");
                newPasswordHash = user.PasswordHash;
            }

            Assert.NotEqual(oldPasswordHash, newPasswordHash);
            Assert.False(await UserManager.CheckPasswordAsync(seededUser, "Test123$"));
            Assert.True(await UserManager.CheckPasswordAsync(seededUser, "Test456$"));
        }

        [Fact]
        public async void EditUserShouldFailForSimplePassword()
        {
            // Given
            UserContextMock.Setup(x => x.IsInRole(RoleNames.HostAdministrator)).Returns(true);
            UserContextMock.Setup(x => x.Id).Returns("1");

            var seededUser = await SeedInitialUserForEdit();
            await UserManager.AddPasswordAsync(seededUser, "Test123$");

            string oldPasswordHash;
            using (var db = GetInMemoryDbContext())
            {
                var user = db.Users.Find("1");
                oldPasswordHash = user.PasswordHash;
            }

            // When
            var result = await ExecuteEditUserQueryAsync(changePassword: true, newPassword: "123");

            // Then
            AssertEditUserQueryFailed(result, error =>
            {
                var exception = Assert.IsType<ExecutionError>(error);
                Assert.Equal("PasswordTooShort: Passwords must be at least 6 characters.", exception.Message);
            });

            string newPasswordHash;
            using (var db = GetInMemoryDbContext())
            {
                var user = db.Users.Find("1");
                newPasswordHash = user.PasswordHash;
            }

            Assert.Equal(oldPasswordHash, newPasswordHash);
            Assert.True(await UserManager.CheckPasswordAsync(seededUser, "Test123$"));
            Assert.False(await UserManager.CheckPasswordAsync(seededUser, "123"));
        }

        private async Threading.Task<User> SeedInitialUserForEdit()
        {
            DbInitializer.SeedTenantRoles(1);

            // Given
            var user = new User
            {
                Id = "1",
                UserName = "BobRay1",
                FirstName = "Bob",
                LastName = "Ray",
                Email = "BobRay@test.com",
                IsActive = false,
                TenantId = 1
            };
            await UserManager.CreateAsync(user);
            await UserManager.AddToRoleAsync(user, RoleNames.User);

            return user;
        }

        private async Threading.Task<ExecutionResult> ExecuteEditUserQueryAsync(bool changePassword = false, string newPassword = "Test456$", bool changeRoles = true)
        {
            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = $@"
                    mutation 
                    {{ 
                        editUser(user: {{ 
                            id: ""1""
                            userName: ""TestUser""
                            firstName: ""Tom""
                            lastName: ""Smith""
                            email: ""test@test.com""
                            isActive: true
                            {(changePassword ? $@"password: ""{newPassword}""" : "")}
                            {(changeRoles ? $@"
                                roles: [
                                    ""{RoleNames.Administrator}""
                                ]" : "")}
                        }})
                        {{ 
                            id
                            userName
                            firstName
                            lastName
                            email
                            isActive
                            roles
                        }}
                    }}"
            });

            return result;
        }

        private void AssertEditUserQueryPassed(ExecutionResult result, bool expectChangedRoles = true)
        {
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                editUserResult => AssertPairEqual(editUserResult,
                    "editUser", user => AssertGraphqlResultDictionary(user,
                        field => AssertPairEqual(field, "id", "1"),
                        field => AssertPairEqual(field, "userName", "TestUser"),
                        field => AssertPairEqual(field, "firstName", "Tom"),
                        field => AssertPairEqual(field, "lastName", "Smith"),
                        field => AssertPairEqual(field, "email", "test@test.com"),
                        field => AssertPairEqual(field, "isActive", true),
                        field => AssertPairEqual(field, "roles",
                            roles => AssertGraphqlResultArray<string>(roles,
                                role => Assert.Equal(role, expectChangedRoles ? RoleNames.Administrator : RoleNames.User)
                            )
                        )
                    )
                )
            );

            using (var db = GetInMemoryDbContext())
            {
                var user = db.Users.Find("1");
                Assert.Equal("TestUser", user.UserName);
                Assert.Equal("Tom", user.FirstName);
                Assert.Equal("Smith", user.LastName);
                Assert.Equal("test@test.com", user.Email);
                Assert.True(user.IsActive);

                var isAdmin = UserManager.IsInRoleAsync(user, RoleNames.Administrator).GetAwaiter().GetResult();
                var isUser = UserManager.IsInRoleAsync(user, RoleNames.User).GetAwaiter().GetResult();

                if (expectChangedRoles)
                {
                    Assert.True(isAdmin);
                    Assert.False(isUser);
                }
                else
                {
                    Assert.False(isAdmin);
                    Assert.True(isUser);
                }
            }
        }

        private void AssertEditUserQueryFailed(ExecutionResult result, params Action<ExecutionError>[] errorInspectors)
        {
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            AssertGraphqlResultDictionary(result.Data,
                editUserResult => AssertPairEqual(editUserResult, "editUser", null)
            );
            Assert.Collection(result.Errors, errorInspectors);

            using (var db = GetInMemoryDbContext())
            {
                var user = db.Users.Find("1");
                Assert.Equal("BobRay1", user.UserName);
                Assert.Equal("Bob", user.FirstName);
                Assert.Equal("Ray", user.LastName);
                Assert.Equal("BobRay@test.com", user.Email);
                Assert.False(user.IsActive);
            }
        }

        [Fact]
        public async void AddProject()
        {
            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = @"
                    mutation 
                    { 
                        addProject(project: { 
                            name: ""Test Project""
                        })
                        { 
                            id
                            name
                        }
                    }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            var projectId = 0;

            AssertGraphqlResultDictionary(result.Data,
                addProjectResult => AssertPairEqual(addProjectResult,
                    "addProject", project => AssertGraphqlResultDictionary(project,
                        field => AssertPairEqual(field, "id", idObject =>
                        {
                            var id = Assert.IsType<int>(idObject);
                            Assert.True(id > 0);
                            projectId = id;
                        }),
                        field => AssertPairEqual(field, "name", "Test Project")
                    )
                )
            );

            Assert.True(projectId > 0);

            using (var db = GetInMemoryDbContext())
            {
                var project = db.Projects.Find(projectId);
                Assert.Equal("Test Project", project.Name);
            }
        }

        [Fact]
        public async void AddProjectShouldFailForNonTenantUser()
        {
            //Given
            UserContextMock.Setup(x => x.TenantId).Returns((int?)null);

            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = @"
                    mutation 
                    { 
                        addProject(project: { 
                            name: ""Test Project""
                        })
                        { 
                            id
                            name
                        }
                    }"
            });

            // Then
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            AssertGraphqlResultDictionary(result.Data,
                addUserResult => AssertPairEqual(addUserResult, "addProject", null)
            );
            Assert.Collection(result.Errors, error =>
            {
                var exception = Assert.IsType<ExecutionError>(error);
                Assert.Equal($"No tenant id. You have to be a member of a tenant to be able to add or edit this record", exception.Message);
            });
        }

        [Fact]
        public async void EditProject()
        {
            // Given
            using (var db = GetInMemoryDbContext())
            {
                db.Projects.Add(new Project { TenantId = 1, Id = 1, Name = "Test Project 1" });
                db.SaveChanges();
            }

            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = @"
                    mutation 
                    { 
                        editProject(project: { 
                            id: 1
                            name: ""Changed name""
                        })
                        { 
                            id
                            name
                        }
                    }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                editProjectResult => AssertPairEqual(editProjectResult,
                    "editProject", project => AssertGraphqlResultDictionary(project,
                        field => AssertPairEqual(field, "id", 1),
                        field => AssertPairEqual(field, "name", "Changed name")
                    )
                )
            );

            using (var db = GetInMemoryDbContext())
            {
                var project = db.Projects.Find(1);
                Assert.Equal("Changed name", project.Name);
            }
        }
        
        [Fact]
        public async void AddTask()
        {
            // Given
            using (var db = GetInMemoryDbContext())
            {
                db.Projects.Add(new Project { TenantId = 1, Id = 1, Name = "Test Project 1" });
                db.SaveChanges();
            }

            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = @"
                    mutation 
                    {
                        addTask(task: { 
                            projectId: 1
                            name: ""Test Task""
                            description: ""Test Description""
                            dueDate: ""2018-09-01""
                            completed: false
                            completionDate: null
                        })
                        { 
                            id
                            name
                            description
                            dueDate
                            completed
                            completionDate
                            project {
                                id
                                name
                            }
                        }
                    }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            var taskId = 0;
            
            AssertGraphqlResultDictionary(result.Data,
                addTaskResult => AssertPairEqual(addTaskResult,
                    "addTask", task => AssertGraphqlResultDictionary(task,
                        field => AssertPairEqual(field, "id", idObject =>
                        {
                            var id = Assert.IsType<int>(idObject);
                            Assert.True(id > 0);
                            taskId = id;
                        }),
                        field => AssertPairEqual(field, "name", "Test Task"),
                        field => AssertPairEqual(field, "description", "Test Description"),
                        field => AssertPairEqual(field, "dueDate", "2018-09-01"),
                        field => AssertPairEqual(field, "completed", false),
                        field => AssertPairEqual(field, "completionDate", null),
                        field => AssertPairEqual(field, "project",
                            project => AssertGraphqlResultDictionary(project,
                                projectField => AssertPairEqual(projectField, "id", 1),
                                projectField => AssertPairEqual(projectField, "name", "Test Project 1")
                            )
                        )
                    )
                )
            );

            Assert.True(taskId > 0);

            using (var db = GetInMemoryDbContext())
            {
                var task = db.Tasks.Find(taskId);
                Assert.Equal(1, task.ProjectId);
                Assert.Equal("Test Task", task.Name);
                Assert.Equal("Test Description", task.Description);
                Assert.Equal(new DateTime(2018, 9, 1), task.DueDate);
                Assert.False(task.Completed);
                Assert.Null(task.CompletionDate);
            }
        }

        [Fact]
        public async void EditTask()
        {
            // Given
            using (var db = GetInMemoryDbContext())
            {
                db.Projects.Add(new Project { TenantId = 1, Id = 1, Name = "Test Project 1" });
                db.Projects.Add(new Project { TenantId = 1, Id = 2, Name = "Test Project 2" });
                db.SaveChanges();
                db.Tasks.Add(new Task { TenantId = 1, Id = 3, ProjectId = 2, Name = "Test Task Query", Description = "Another test task", DueDate = new DateTime(2000, 1, 1), Completed = true, CompletionDate = new DateTime(2010, 1, 1) });
                db.SaveChanges();
            }

            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = @"
                    mutation 
                    { 
                        editTask(task: {
                            id: 3
                            projectId: 1
                            name: ""Test Task""
                            description: ""Test Description""
                            dueDate: ""2018-09-01""
                            completed: false
                            completionDate: null
                        })
                        { 
                            id
                            name
                            description
                            dueDate
                            completed
                            completionDate
                            project {
                                id
                                name
                            }
                        }
                    }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                editTaskResult => AssertPairEqual(editTaskResult,
                    "editTask", task => AssertGraphqlResultDictionary(task,
                        field => AssertPairEqual(field, "id", 3),
                        field => AssertPairEqual(field, "name", "Test Task"),
                        field => AssertPairEqual(field, "description", "Test Description"),
                        field => AssertPairEqual(field, "dueDate", "2018-09-01"),
                        field => AssertPairEqual(field, "completed", false),
                        field => AssertPairEqual(field, "completionDate", null),
                        field => AssertPairEqual(field, "project",
                            project => AssertGraphqlResultDictionary(project,
                                projectField => AssertPairEqual(projectField, "id", 1),
                                projectField => AssertPairEqual(projectField, "name", "Test Project 1")
                            )
                        )
                    )
                )
            );

            using (var db = GetInMemoryDbContext())
            {
                var task = db.Tasks.Find(3);
                Assert.Equal(1, task.ProjectId);
                Assert.Equal("Test Task", task.Name);
                Assert.Equal("Test Description", task.Description);
                Assert.Equal(new DateTime(2018, 9, 1), task.DueDate);
                Assert.False(task.Completed);
                Assert.Null(task.CompletionDate);
            }
        }
    }
}
