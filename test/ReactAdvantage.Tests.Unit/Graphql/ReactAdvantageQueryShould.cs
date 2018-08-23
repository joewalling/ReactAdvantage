using System.Collections.Generic;
using ReactAdvantage.Api.GraphQLSchema;
using ReactAdvantage.Domain.Models;
using Xunit;

namespace ReactAdvantage.Tests.Unit.Graphql
{
    public class ReactAdvantageQueryShould : GraphqlTestBase
    {
        [Fact]
        public async void ReturnUser()
        {
            using (var db = GetInMemoryDbContext())
            {
                db.Users.Add(new User { Id = 1, Name = "Admin" });
                db.Users.Add(new User { Id = 2, Name = "Test" });
                db.SaveChanges();
            }

            var result = await BuildSchemaAndExecuteQueryAsync(new GraphQLQuery
            {
                Query = "query { user(id: 1) { id name } }"
            });

            Assert.NotNull(result);
            Assert.False(result.Errors?.Count > 0, "Result should contain no errors");
            Assert.NotNull(result.Data);
            var data = Assert.IsType<Dictionary<string, object>>(result.Data);
            var dataElement = Assert.Single(data);
            Assert.Equal("user", dataElement.Key);
            var userFields = Assert.IsType<Dictionary<string, object>>(dataElement.Value);
            Assert.Equal(2, userFields.Count);
            Assert.Collection(userFields,
                id => 
                {
                    Assert.Equal("id", id.Key);
                    Assert.Equal(1, id.Value);
                },
                name =>
                {
                    Assert.Equal("name", name.Key);
                    Assert.Equal("Admin", name.Value);
                });
        }

        
    }
}
