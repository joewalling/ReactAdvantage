using System.Security.Claims;
using GraphQL;
using Moq;
using ReactAdvantage.Api.GraphQLSchema;
using Xunit;

namespace ReactAdvantage.Tests.Unit.Api.Graphql
{
    public class GraphQLUserContextShould
    {
        [Fact]
        public void ReturnId()
        {
            //Given
            var claimsPrincipal = new Mock<ClaimsPrincipal>();
            claimsPrincipal.Setup(x => x.Claims).Returns(new[] { new Claim("sub", "testId") });

            var userContext = new GraphQLUserContext(claimsPrincipal.Object);

            //When
            var id = userContext.Id;

            //Then
            Assert.Equal("testId", id);
        }

        [Fact]
        public void NotThrowOnMissingIdClaim()
        {
            //Given
            var claimsPrincipal = new Mock<ClaimsPrincipal>();
            claimsPrincipal.Setup(x => x.Claims).Returns(new[] { new Claim("notSub", "test value") });

            var userContext = new GraphQLUserContext(claimsPrincipal.Object);

            //When
            var id = userContext.Id;

            //Then
            Assert.Null(id);
        }

        [Fact]
        public void ReturnIdForEmptyUser()
        {
            //Given
            var userContext = new GraphQLUserContext();

            //When
            var id = userContext.Id;

            //Then
            Assert.Null(id);
        }

        private Mock<ClaimsPrincipal> GetClaimPrincipalMockForRoles()
        {
            var claimsPrincipal = new Mock<ClaimsPrincipal>();
            claimsPrincipal.Setup(x => x.IsInRole("GrantedRole")).Returns(true);
            claimsPrincipal.Setup(x => x.IsInRole("NotGrantedRole")).Returns(false);

            return claimsPrincipal;
        }

        [Fact]
        public void ReturnIsInRoleForUser()
        {
            //Given
            var claimsPrincipal = GetClaimPrincipalMockForRoles();
            var userContext = new GraphQLUserContext(claimsPrincipal.Object);

            //When
            var isInRole = userContext.IsInRole("GrantedRole");

            //Then
            Assert.True(isInRole);
        }

        [Fact]
        public void ReturnIsNotInRoleForUser()
        {
            //Given
            var claimsPrincipal = GetClaimPrincipalMockForRoles();
            var userContext = new GraphQLUserContext(claimsPrincipal.Object);

            //When
            var isInRole = userContext.IsInRole("NotGrantedRole");

            //Then
            Assert.False(isInRole);
        }

        [Fact]
        public void ReturnIsNotInRoleForEmptyUser()
        {
            //Given
            var userContext = new GraphQLUserContext();

            //When
            var isInRole = userContext.IsInRole("AnyRole");

            //Then
            Assert.False(isInRole);
        }

        [Fact]
        public void ThrowOnNotInRole()
        {
            //Given
            var claimsPrincipal = GetClaimPrincipalMockForRoles();
            var userContext = new GraphQLUserContext(claimsPrincipal.Object);

            //Then
            var exception = Assert.Throws<ExecutionError>(() =>
            {
                //When
                userContext.EnsureIsInRole("NotGrantedRole");
            });

            Assert.Equal("Unauthorized. You have to be a member of NotGrantedRole role.", exception.Message);
        }

        [Fact]
        public void NotThrowOnInRole()
        {
            //Given
            var claimsPrincipal = GetClaimPrincipalMockForRoles();
            var userContext = new GraphQLUserContext(claimsPrincipal.Object);

            //When
            userContext.EnsureIsInRole("GrantedRole");
        }
    }
}
