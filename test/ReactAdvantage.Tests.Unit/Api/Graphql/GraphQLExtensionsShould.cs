using GraphQL;
using Microsoft.AspNetCore.Identity;
using ReactAdvantage.Api.GraphQLSchema;
using Xunit;

namespace ReactAdvantage.Tests.Unit.Api.Graphql
{
    public class GraphQLExtensionsShould
    {
        [Fact]
        public void ThrowOnError()
        {
            //Given
            var identityResult = IdentityResult.Failed();

            //Then
            var exception = Assert.Throws<ExecutionError>(() =>
            {
                //When
                identityResult.ThrowOnError();
            });

            Assert.Equal("Identity error", exception.Message);
        }

        [Fact]
        public void ThrowOnErrorWithDetails()
        {
            //Given
            var identityResult = IdentityResult.Failed(new IdentityError { Code = "TestCode", Description = "Test Description" });

            //Then
            var exception = Assert.Throws<ExecutionError>(() =>
            {
                //When
                identityResult.ThrowOnError();
            });

            Assert.Equal("TestCode: Test Description", exception.Message);
        }

        [Fact]
        public void NotThrowOnError()
        {
            //Given
            var identityResult = IdentityResult.Success;

            //When
            identityResult.ThrowOnError();
        }
    }
}
