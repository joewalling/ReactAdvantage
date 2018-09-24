using ReactAdvantage.Api.GraphQLSchema;
using ReactAdvantage.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ReactAdvantage.Tests.Unit.Api.Graphql
{
    public class ReactAdvantageMutationShould : GraphqlTestBase
    {
        [Fact]
        public async void AddUser()
        {
            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = @"
                    mutation 
                    { 
                        addUser(user: { 
                            userName: ""TestUser""
                            firstName: ""Tom""
                            lastName: ""Smith""
                            email: ""test@test.com""
                            isActive: true
                        })
                        { 
                            id
                            userName
                            firstName
                            lastName
                            email
                            isActive
                        }
                    }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

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
                        field => AssertPairEqual(field, "isActive", true)
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
        }

        [Fact]
        public async void EditUser()
        {
            // Given
            var userManager = ServiceProvider.GetService<UserManager<User>>();
            userManager.CreateAsync(new User { Id = "1", UserName = "BobRay1", FirstName = "Bob", LastName = "Ray", Email = "BobRay@test.com", IsActive = false })
                .GetAwaiter().GetResult().ThrowOnError();

            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = @"
                    mutation 
                    { 
                        editUser(user: { 
                            id: ""1""
                            userName: ""TestUser""
                            firstName: ""Tom""
                            lastName: ""Smith""
                            email: ""test@test.com""
                            isActive: true
                        })
                        { 
                            id
                            userName
                            firstName
                            lastName
                            email
                            isActive
                        }
                    }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                editUserResult => AssertPairEqual(editUserResult,
                    "editUser", user => AssertGraphqlResultDictionary(user,
                        field => AssertPairEqual(field, "id", "1"),
                        field => AssertPairEqual(field, "userName", "TestUser"),
                        field => AssertPairEqual(field, "firstName", "Tom"),
                        field => AssertPairEqual(field, "lastName", "Smith"),
                        field => AssertPairEqual(field, "email", "test@test.com"),
                        field => AssertPairEqual(field, "isActive", true)
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
        public async void EditProject()
        {
            // Given
            using (var db = GetInMemoryDbContext())
            {
                db.Projects.Add(new Project { Id = 1, Name = "Test Project 1" });
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
                db.Projects.Add(new Project { Id = 1, Name = "Test Project 1" });
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
                db.Projects.Add(new Project { Id = 1, Name = "Test Project 1" });
                db.Projects.Add(new Project { Id = 2, Name = "Test Project 2" });
                db.SaveChanges();
                db.Tasks.Add(new Task { Id = 3, ProjectId = 2, Name = "Test Task Query", Description = "Another test task", DueDate = new DateTime(2000, 1, 1), Completed = true, CompletionDate = new DateTime(2010, 1, 1) });
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
