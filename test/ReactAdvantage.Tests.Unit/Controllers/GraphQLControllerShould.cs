using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReactAdvantage.Api.Controllers;
using ReactAdvantage.Api.GraphQLSchema;
using Xunit;

namespace ReactAdvantage.Tests.Unit.Controllers
{
    public class GraphQLControllerShould
    {
        private readonly GraphQLController _graphqlController;

        public GraphQLControllerShould()
        {
            // Given
            var documentExecutor = new Mock<IDocumentExecuter>();
            documentExecutor.Setup(x => x.ExecuteAsync(It.IsAny<ExecutionOptions>())).Returns(Task.FromResult(new ExecutionResult()));
            var schema = new Mock<ISchema>();
            _graphqlController = new GraphQLController(schema.Object, documentExecutor.Object);
        }

        [Fact]
        public async void ReturnNotNullExecutionResult()
        {
            // Given
            var query = new GraphQLQuery { Query = @"{ query { users { id name } } }" };

            // When
            var result = await _graphqlController.Post(query);

            // Then
            Assert.NotNull(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var executionResult = okObjectResult.Value;
            Assert.NotNull(executionResult);
        }
    }
}
