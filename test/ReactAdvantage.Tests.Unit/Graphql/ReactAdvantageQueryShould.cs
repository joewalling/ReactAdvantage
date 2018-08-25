using System.Linq;
using ReactAdvantage.Api.GraphQLSchema;
using ReactAdvantage.Domain.Models;
using Xunit;

namespace ReactAdvantage.Tests.Unit.Graphql
{
    public class ReactAdvantageQueryShould : GraphqlTestBase
    {
        public ReactAdvantageQueryShould()
        {
            // Given
            using (var db = GetInMemoryDbContext())
            {
                db.Users.Add(new User { Id = 1, Name = "BobRay1", FirstName = "Bob", LastName = "Ray", Email = "BobRay@test.com", IsActive = true });
                db.Users.Add(new User { Id = 2, Name = "BobRay2", FirstName = "Bob", LastName = "Ray", Email = "BobRay@test.com", IsActive = false });
                db.Users.Add(new User { Id = 3, Name = "BobRay3", FirstName = "Bob", LastName = "Ray", Email = "BobRay@test2.com", IsActive = true });
                db.Users.Add(new User { Id = 4, Name = "BobRay4", FirstName = "Bob", LastName = "Ray", Email = "BobRay@test2.com", IsActive = false });
                db.Users.Add(new User { Id = 5, Name = "BobSmith1", FirstName = "Bob", LastName = "Smith", Email = "BobSmith@test.com", IsActive = true });
                db.Users.Add(new User { Id = 6, Name = "BobSmith2", FirstName = "Bob", LastName = "Smith", Email = "BobSmith@test.com", IsActive = false });
                db.Users.Add(new User { Id = 7, Name = "BobSmith3", FirstName = "Bob", LastName = "Smith", Email = "BobSmith@test2.com", IsActive = true });
                db.Users.Add(new User { Id = 8, Name = "BobSmith4", FirstName = "Bob", LastName = "Smith", Email = "BobSmith@test2.com", IsActive = false });
                db.Users.Add(new User { Id = 9, Name = "BarbaraRay1", FirstName = "Barbara", LastName = "Ray", Email = "BarbaraRay@test.com", IsActive = true });
                db.Users.Add(new User { Id = 10, Name = "BarbaraRay2", FirstName = "Barbara", LastName = "Ray", Email = "BarbaraRay@test.com", IsActive = false });
                db.Users.Add(new User { Id = 11, Name = "BarbaraRay3", FirstName = "Barbara", LastName = "Ray", Email = "BarbaraRay@test2.com", IsActive = true });
                db.Users.Add(new User { Id = 12, Name = "BarbaraRay4", FirstName = "Barbara", LastName = "Ray", Email = "BarbaraRay@test2.com", IsActive = false });
                db.Users.Add(new User { Id = 13, Name = "BarbaraSmith1", FirstName = "Barbara", LastName = "Smith", Email = "BarbaraSmith@test.com", IsActive = true });
                db.Users.Add(new User { Id = 14, Name = "BarbaraSmith2", FirstName = "Barbara", LastName = "Smith", Email = "BarbaraSmith@test.com", IsActive = false });
                db.Users.Add(new User { Id = 15, Name = "BarbaraSmith3", FirstName = "Barbara", LastName = "Smith", Email = "BarbaraSmith@test2.com", IsActive = true });
                db.Users.Add(new User { Id = 16, Name = "BarbaraSmith4", FirstName = "Barbara", LastName = "Smith", Email = "BarbaraSmith@test2.com", IsActive = false });
                db.SaveChanges();
            }
        }

        [Fact]
        public async void ReturnUserWithFewFields()
        {
            // When
            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = "query { user(id: 1) { id name } }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                userResult => AssertPairEqual(userResult, 
                    "user", user => AssertGraphqlResultDictionary(user,
                        field => AssertPairEqual(field, "id", 1),
                        field => AssertPairEqual(field, "name", "BobRay1")
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
                Query = "query { user(id: 16) { id name firstName lastName email isActive } }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                userResult => AssertPairEqual(userResult,
                    "user", user => AssertGraphqlResultDictionary(user,
                        field => AssertPairEqual(field, "id", 16),
                        field => AssertPairEqual(field, "name", "BarbaraSmith4"),
                        field => AssertPairEqual(field, "firstName", "Barbara"),
                        field => AssertPairEqual(field, "lastName", "Smith"),
                        field => AssertPairEqual(field, "email", "BarbaraSmith@test2.com"),
                        field => AssertPairEqual(field, "isActive", false)
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
                Query = "query { users(firstname: \"Bo\", lastname: \"Sm\", email: \"@test2.com\", isactive: true) { id name firstName lastName email isActive } }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                usersResult => AssertPairEqual(usersResult,
                    "users", users => AssertGraphqlResultArray(users, 
                        user => AssertGraphqlResultDictionary(user,
                            field => AssertPairEqual(field, "id", 7),
                            field => AssertPairEqual(field, "name", "BobSmith3"),
                            field => AssertPairEqual(field, "firstName", "Bob"),
                            field => AssertPairEqual(field, "lastName", "Smith"),
                            field => AssertPairEqual(field, "email", "BobSmith@test2.com"),
                            field => AssertPairEqual(field, "isActive", true)
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
                Query = "query { users(id: [1, 2, 3], isactive: true, ) { id name firstName lastName email isActive } }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                userResult => AssertPairEqual(userResult,
                    "users", users => AssertGraphqlResultArray(users,
                        user => AssertGraphqlResultDictionary(user,
                            field => AssertPairEqual(field, "id", 1),
                            field => AssertPairEqual(field, "name", "BobRay1"),
                            field => AssertPairEqual(field, "firstName", "Bob"),
                            field => AssertPairEqual(field, "lastName", "Ray"),
                            field => AssertPairEqual(field, "email", "BobRay@test.com"),
                            field => AssertPairEqual(field, "isActive", true)
                        ),
                        user => AssertGraphqlResultDictionary(user,
                            field => AssertPairEqual(field, "id", 3),
                            field => AssertPairEqual(field, "name", "BobRay3"),
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
                Query = "query { users { id name } }"
            });

            // Then
            AssertValidGraphqlExecutionResult(result);

            AssertGraphqlResultDictionary(result.Data,
                userResult => AssertPairEqual(userResult,
                    "users", users =>
                    {
                        var usersArray = Assert.IsType<object[]>(users);
                        Assert.Equal(16, usersArray.Length);
                        AssertGraphqlResultDictionary(usersArray.First(),
                            field => AssertPairEqual(field, "id", 1),
                            field => AssertPairEqual(field, "name", "BobRay1")
                        );
                    }
                )
            );
        }


    }
}
