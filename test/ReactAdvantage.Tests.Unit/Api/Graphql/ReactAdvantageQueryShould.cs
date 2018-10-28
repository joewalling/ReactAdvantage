using System;
using System.Linq;
using ReactAdvantage.Api.GraphQLSchema;
using ReactAdvantage.Domain.Configuration;
using ReactAdvantage.Domain.Models;
using Xunit;

namespace ReactAdvantage.Tests.Unit.Api.Graphql
{
    public class ReactAdvantageQueryShould : GraphqlTestBase
    {
        public ReactAdvantageQueryShould()
        {
            // Given
            using (var db = GetInMemoryDbContext())
            {
                DbInitializer.SeedTenantRoles(1);

                var users = new[]
                {
                    new User { TenantId = 1, Id = "1", UserName = "BobRay1", FirstName = "Bob", LastName = "Ray", Email = "BobRay@test.com", IsActive = true },
                    new User { TenantId = 1, Id = "2", UserName = "BobRay2", FirstName = "Bob", LastName = "Ray", Email = "BobRay@test.com", IsActive = false },
                    new User { TenantId = 1, Id = "3", UserName = "BobRay3", FirstName = "Bob", LastName = "Ray", Email = "BobRay@test2.com", IsActive = true },
                    new User { TenantId = 1, Id = "4", UserName = "BobRay4", FirstName = "Bob", LastName = "Ray", Email = "BobRay@test2.com", IsActive = false },
                    new User { TenantId = 1, Id = "5", UserName = "BobSmith1", FirstName = "Bob", LastName = "Smith", Email = "BobSmith@test.com", IsActive = true },
                    new User { TenantId = 1, Id = "6", UserName = "BobSmith2", FirstName = "Bob", LastName = "Smith", Email = "BobSmith@test.com", IsActive = false },
                    new User { TenantId = 1, Id = "7", UserName = "BobSmith3", FirstName = "Bob", LastName = "Smith", Email = "BobSmith@test2.com", IsActive = true },
                    new User { TenantId = 1, Id = "8", UserName = "BobSmith4", FirstName = "Bob", LastName = "Smith", Email = "BobSmith@test2.com", IsActive = false },
                    new User { TenantId = 1, Id = "9", UserName = "BarbaraRay1", FirstName = "Barbara", LastName = "Ray", Email = "BarbaraRay@test.com", IsActive = true },
                    new User { TenantId = 1, Id = "10", UserName = "BarbaraRay2", FirstName = "Barbara", LastName = "Ray", Email = "BarbaraRay@test.com", IsActive = false },
                    new User { TenantId = 1, Id = "11", UserName = "BarbaraRay3", FirstName = "Barbara", LastName = "Ray", Email = "BarbaraRay@test2.com", IsActive = true },
                    new User { TenantId = 1, Id = "12", UserName = "BarbaraRay4", FirstName = "Barbara", LastName = "Ray", Email = "BarbaraRay@test2.com", IsActive = false },
                    new User { TenantId = 1, Id = "13", UserName = "BarbaraSmith1", FirstName = "Barbara", LastName = "Smith", Email = "BarbaraSmith@test.com", IsActive = true },
                    new User { TenantId = 1, Id = "14", UserName = "BarbaraSmith2", FirstName = "Barbara", LastName = "Smith", Email = "BarbaraSmith@test.com", IsActive = false },
                    new User { TenantId = 1, Id = "15", UserName = "BarbaraSmith3", FirstName = "Barbara", LastName = "Smith", Email = "BarbaraSmith@test2.com", IsActive = true },
                    new User { TenantId = 1, Id = "16", UserName = "BarbaraSmith4", FirstName = "Barbara", LastName = "Smith", Email = "BarbaraSmith@test2.com", IsActive = false }
                };

                foreach (var user in users)
                {
                    UserManager.CreateAsync(user).GetAwaiter().GetResult();
                }

                UserManager.AddToRoleAsync(users.First(x => x.Id == "7"), RoleNames.Administrator).GetAwaiter().GetResult();
                UserManager.AddToRoleAsync(users.First(x => x.Id == "16"), RoleNames.User).GetAwaiter().GetResult();
                
                db.Projects.Add(new Project { TenantId = 1, Id = 1, Name = "Test Project 1" });
                db.Projects.Add(new Project { TenantId = 1, Id = 2, Name = "Test Project 2" });
                db.Projects.Add(new Project { TenantId = 1, Id = 3, Name = "Another Project 3" });
                db.SaveChanges();

                db.Tasks.Add(new Task { TenantId = 1, Id = 1, ProjectId = 1, Name = "Task 1", Description = "This is a test task", DueDate = new DateTime(2020, 1, 1) });
                db.Tasks.Add(new Task { TenantId = 1, Id = 2, ProjectId = 1, Name = "Task 2", Description = "Another test task", DueDate = new DateTime(2000, 1, 1), Completed = true, CompletionDate = new DateTime(2010, 1, 1) });
                db.Tasks.Add(new Task { TenantId = 1, Id = 3, ProjectId = 1, Name = "Test Task Query", Description = "Another test task", DueDate = new DateTime(2000, 1, 1), Completed = true, CompletionDate = new DateTime(2010, 1, 1) });
                db.Tasks.Add(new Task { TenantId = 1, Id = 4, ProjectId = 2, Name = "Test Task Query", Description = "Another test task", DueDate = new DateTime(2000, 1, 1), Completed = true, CompletionDate = new DateTime(2010, 1, 1) });
                db.Tasks.Add(new Task { TenantId = 1, Id = 5, ProjectId = 1, Name = "Test Task Query", Description = "Another test task", DueDate = new DateTime(2001, 1, 1), Completed = true, CompletionDate = new DateTime(2010, 1, 1) });
                db.Tasks.Add(new Task { TenantId = 1, Id = 6, ProjectId = 1, Name = "Test Task Query", Description = "Another test task", DueDate = new DateTime(2000, 1, 1), Completed = false, CompletionDate = new DateTime(2010, 1, 1) });
                db.Tasks.Add(new Task { TenantId = 1, Id = 7, ProjectId = 1, Name = "Test Task Query", Description = "Another test task", DueDate = new DateTime(2000, 1, 1), Completed = true, CompletionDate = new DateTime(2011, 1, 1) });
                db.Tasks.Add(new Task { TenantId = 1, Id = 8, ProjectId = 1, Name = "Test Task Query", Description = "Another test task", DueDate = new DateTime(2000, 1, 1), Completed = true, CompletionDate = null });
                db.Tasks.Add(new Task { TenantId = 1, Id = 9, ProjectId = 1, Name = "Test Task Query", Description = "Another test task", DueDate = new DateTime(2000, 1, 1), Completed = true, CompletionDate = new DateTime(2010, 1, 1) });
                db.SaveChanges();
            }
        }

        [Fact]
        public async void ReturnUserWithFewFields()
        {
            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = "query { user(id: \"1\") { id userName } }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                userResult => AssertPairEqual(userResult, 
                    "user", user => AssertGraphqlResultDictionary(user,
                        field => AssertPairEqual(field, "id", "1"),
                        field => AssertPairEqual(field, "userName", "BobRay1")
                    )
                )
            );
        }

        [Fact]
        public async void ReturnUserWithMoreFields()
        {
            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = "query { user(id: \"16\") { id userName firstName lastName email isActive roles } }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                userResult => AssertPairEqual(userResult,
                    "user", user => AssertGraphqlResultDictionary(user,
                        field => AssertPairEqual(field, "id", "16"),
                        field => AssertPairEqual(field, "userName", "BarbaraSmith4"),
                        field => AssertPairEqual(field, "firstName", "Barbara"),
                        field => AssertPairEqual(field, "lastName", "Smith"),
                        field => AssertPairEqual(field, "email", "BarbaraSmith@test2.com"),
                        field => AssertPairEqual(field, "isActive", false),
                        field => AssertPairEqual(field, "roles", 
                            roles => AssertGraphqlResultArray<string>(roles,
                                role => Assert.Equal(role, RoleNames.User)
                            )
                        )
                    )
                )
            );
        }

        [Fact]
        public async void ReturnQueriedUsers()
        {
            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = "query { users(firstname: \"Bo\", lastname: \"Sm\", email: \"@test2.com\", isactive: true) { id userName firstName lastName email isActive roles } }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                usersResult => AssertPairEqual(usersResult,
                    "users", users => AssertGraphqlResultArray(users, 
                        user => AssertGraphqlResultDictionary(user,
                            field => AssertPairEqual(field, "id", "7"),
                            field => AssertPairEqual(field, "userName", "BobSmith3"),
                            field => AssertPairEqual(field, "firstName", "Bob"),
                            field => AssertPairEqual(field, "lastName", "Smith"),
                            field => AssertPairEqual(field, "email", "BobSmith@test2.com"),
                            field => AssertPairEqual(field, "isActive", true),
                            field => AssertPairEqual(field, "roles",
                                roles => AssertGraphqlResultArray<string>(roles,
                                    role => Assert.Equal(role, RoleNames.Administrator)
                                )
                            )
                        )
                    )
                )
            );
        }

        [Fact]
        public async void ReturnQueriedUsersById()
        {
            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = "query { users(id: [\"1\", \"2\", \"3\"], isactive: true, ) { id userName firstName lastName email isActive } }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                usersResult => AssertPairEqual(usersResult,
                    "users", users => AssertGraphqlResultArray(users,
                        user => AssertGraphqlResultDictionary(user,
                            field => AssertPairEqual(field, "id", "1"),
                            field => AssertPairEqual(field, "userName", "BobRay1"),
                            field => AssertPairEqual(field, "firstName", "Bob"),
                            field => AssertPairEqual(field, "lastName", "Ray"),
                            field => AssertPairEqual(field, "email", "BobRay@test.com"),
                            field => AssertPairEqual(field, "isActive", true)
                        ),
                        user => AssertGraphqlResultDictionary(user,
                            field => AssertPairEqual(field, "id", "3"),
                            field => AssertPairEqual(field, "userName", "BobRay3"),
                            field => AssertPairEqual(field, "firstName", "Bob"),
                            field => AssertPairEqual(field, "lastName", "Ray"),
                            field => AssertPairEqual(field, "email", "BobRay@test2.com"),
                            field => AssertPairEqual(field, "isActive", true)
                        )
                    )
                )
            );
        }

        [Fact]
        public async void ReturnAllUsers()
        {
            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = "query { users { id userName } }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                usersResult => AssertPairEqual(usersResult,
                    "users", users =>
                    {
                        var usersArray = Assert.IsType<object[]>(users);
                        Assert.Equal(16, usersArray.Length);
                        AssertGraphqlResultDictionary(usersArray.First(),
                            field => AssertPairEqual(field, "id", "1"),
                            field => AssertPairEqual(field, "userName", "BobRay1")
                        );
                    }
                )
            );
        }

        [Fact]
        public async void ReturnAllRoles()
        {
            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = "query { roles { name isStatic } }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                rolesResult => AssertPairEqual(rolesResult,
                    "roles", roles => AssertGraphqlResultArray(roles,
                        role => AssertGraphqlResultDictionary(role,
                            field => AssertPairEqual(field, "name", RoleNames.Administrator),
                            field => AssertPairEqual(field, "isStatic", true)
                        ),
                        role => AssertGraphqlResultDictionary(role,
                            field => AssertPairEqual(field, "name", RoleNames.User),
                            field => AssertPairEqual(field, "isStatic", true)
                        )
                    )
                )
            );
        }

        [Fact]
        public async void ReturnProject()
        {
            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = "query { project(id: 2) { id name } }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                projectResult => AssertPairEqual(projectResult,
                    "project", project => AssertGraphqlResultDictionary(project,
                        field => AssertPairEqual(field, "id", 2),
                        field => AssertPairEqual(field, "name", "Test Project 2")
                    )
                )
            );
        }

        [Fact]
        public async void ReturnAllProjects()
        {
            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = "query { projects { id name } }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);
            
            AssertGraphqlResultDictionary(result.Data,
                projectsResult => AssertPairEqual(projectsResult,
                    "projects", projects =>
                    {
                        var projectsArray = Assert.IsType<object[]>(projects);
                        Assert.Equal(3, projectsArray.Length);
                        AssertGraphqlResultDictionary(projectsArray.First(),
                            field => AssertPairEqual(field, "id", 1),
                            field => AssertPairEqual(field, "name", "Test Project 1")
                        );
                    }
                )
            );
        }

        [Fact]
        public async void ReturnQueriedProjects()
        {
            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = "query { projects(id: [2, 3], name: \"Test\") { id name } }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                projectsResult => AssertPairEqual(projectsResult,
                    "projects", projects => AssertGraphqlResultArray(projects,
                        project => AssertGraphqlResultDictionary(project,
                            field => AssertPairEqual(field, "id", 2),
                            field => AssertPairEqual(field, "name", "Test Project 2")
                        )
                    )
                )
            );
        }

        [Fact]
        public async void ReturnTask()
        {
            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = "query { task(id: 2) { id name description dueDate completed completionDate project { id name } } }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                taskResult => AssertPairEqual(taskResult,
                    "task", task => AssertGraphqlResultDictionary(task,
                        field => AssertPairEqual(field, "id", 2),
                        field => AssertPairEqual(field, "name", "Task 2"),
                        field => AssertPairEqual(field, "description", "Another test task"),
                        field => AssertPairEqual(field, "dueDate", "2000-01-01"),
                        field => AssertPairEqual(field, "completed", true),
                        field => AssertPairEqual(field, "completionDate", "2010-01-01"),
                        field => AssertPairEqual(field, "project",
                            project => AssertGraphqlResultDictionary(project,
                                projectField => AssertPairEqual(projectField, "id", 1),
                                projectField => AssertPairEqual(projectField, "name", "Test Project 1")
                            )
                        )
                    )
                )
            );
        }

        [Fact]
        public async void ReturnAllTasks()
        {
            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = "query { tasks { id name } }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                tasksResult => AssertPairEqual(tasksResult,
                    "tasks", tasks =>
                    {
                        var tasksArray = Assert.IsType<object[]>(tasks);
                        Assert.Equal(9, tasksArray.Length);
                        AssertGraphqlResultDictionary(tasksArray.First(),
                            field => AssertPairEqual(field, "id", 1),
                            field => AssertPairEqual(field, "name", "Task 1")
                        );
                    }
                )
            );
        }

        [Fact]
        public async void ReturnQueriedTasks()
        {
            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = "query { tasks(id: [1, 2, 3, 4, 5, 6, 7, 8], name: \"Test Task Query\", projectid: 1, iscompleted: true, duedate: \"2000-01-01\", completiondate: \"2010-01-01\") { id } }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                projectsResult => AssertPairEqual(projectsResult,
                    "tasks", tasks => AssertGraphqlResultArray(tasks,
                        task => AssertGraphqlResultDictionary(task,
                            field => AssertPairEqual(field, "id", 3)
                        )
                    )
                )
            );
        }

        [Fact]
        public async void ReturnTwoQueries()
        {
            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = "query { user(id: \"1\") { userName }, project(id: 1) { name } }"
            });
            
            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                userResult => AssertPairEqual(userResult,
                    "user", user => AssertGraphqlResultDictionary(user,
                        field => AssertPairEqual(field, "userName", "BobRay1")
                    )
                ),
                projectResult => AssertPairEqual(projectResult,
                    "project", project => AssertGraphqlResultDictionary(project,
                        field => AssertPairEqual(field, "name", "Test Project 1")
                    )
                )
            );
        }
    }
}
