using ReactAdvantage.Api.GraphQLSchema;
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
    }
}
