using ReactAdvantage.Api.GraphQLSchema;
using ReactAdvantage.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ReactAdvantage.Tests.Unit.Graphql
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
                            name: ""Test User""
                            firstName: ""Tom""
                            lastName: ""Smith""
                            email: ""test@test.com""
                            isActive: true
                        })
                        { 
                            id
                            name
                            firstName
                            lastName
                            email
                            isActive
                        }
                    }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            var userId = 0;

            AssertGraphqlResultDictionary(result.Data,
                addUserResult => AssertPairEqual(addUserResult,
                    "addUser", user => AssertGraphqlResultDictionary(user,
                        field => AssertPairEqual(field, "id", idObject =>
                        {
                            var id = Assert.IsType<int>(idObject);
                            Assert.True(id > 0);
                            userId = id;
                        }),
                        field => AssertPairEqual(field, "name", "Test User"),
                        field => AssertPairEqual(field, "firstName", "Tom"),
                        field => AssertPairEqual(field, "lastName", "Smith"),
                        field => AssertPairEqual(field, "email", "test@test.com"),
                        field => AssertPairEqual(field, "isActive", true)
                    )
                )
            );

            Assert.True(userId > 0);

            using (var db = GetInMemoryDbContext())
            {
                var user = db.Users.Find(userId);
                Assert.Equal("Test User", user.Name);
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
            using (var db = GetInMemoryDbContext())
            {
                db.Users.Add(new User { Id = 1, Name = "BobRay1", FirstName = "Bob", LastName = "Ray", Email = "BobRay@test.com", IsActive = false });
                db.SaveChanges();
            }

            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = @"
                    mutation 
                    { 
                        editUser(user: { 
                            id: 1
                            name: ""Test User""
                            firstName: ""Tom""
                            lastName: ""Smith""
                            email: ""test@test.com""
                            isActive: true
                        })
                        { 
                            id
                            name
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
                        field => AssertPairEqual(field, "id", 1),
                        field => AssertPairEqual(field, "name", "Test User"),
                        field => AssertPairEqual(field, "firstName", "Tom"),
                        field => AssertPairEqual(field, "lastName", "Smith"),
                        field => AssertPairEqual(field, "email", "test@test.com"),
                        field => AssertPairEqual(field, "isActive", true)
                    )
                )
            );

            using (var db = GetInMemoryDbContext())
            {
                var user = db.Users.Find(1);
                Assert.Equal("Test User", user.Name);
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
    }
}
