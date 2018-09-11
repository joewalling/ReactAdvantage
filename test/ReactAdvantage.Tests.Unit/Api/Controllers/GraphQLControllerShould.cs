using System;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ReactAdvantage.Api.Controllers;
using ReactAdvantage.Api.GraphQLSchema;
using Xunit;

namespace ReactAdvantage.Tests.Unit.Api.Controllers
{
    public class GraphQLControllerShould
    {
        private readonly GraphQLController _graphqlController;
        private readonly Mock<ISchema> _schemaMock;
        private readonly Mock<IDocumentExecuter> _documentExecutorMock;
        private readonly Mock<ILogger<GraphQLController>> _loggerMock;

        public GraphQLControllerShould()
        {
            // Given
            _documentExecutorMock = new Mock<IDocumentExecuter>();
            _documentExecutorMock.Setup(x => x.ExecuteAsync(It.IsAny<ExecutionOptions>())).Returns(Task.FromResult(new ExecutionResult()));
            _schemaMock = new Mock<ISchema>();
            _loggerMock = new Mock<ILogger<GraphQLController>>();
            _graphqlController = new GraphQLController(_schemaMock.Object, _documentExecutorMock.Object, _loggerMock.Object);
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
            Assert.IsType<ExecutionResult>(executionResult);
        }

        [Fact]
        public async void ThrowArgumentNullException()
        {
            // Then
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // When
                await _graphqlController.Post(null);
            });
        }

        [Fact]
        public async void ReturnBadResultOnError()
        {
            //Given
            var expectedExecutionResult = new ExecutionResult
            {
                Errors = new ExecutionErrors { new ExecutionError("", new Exception()) }
            };

            _documentExecutorMock
                .Setup(x => x.ExecuteAsync(It.IsAny<ExecutionOptions>()))
                .Returns(Task.FromResult(expectedExecutionResult));

            var query = new GraphQLQuery { Query = @"{ query { users { id name } } }" };

            // When
            var result = await _graphqlController.Post(query);

            // Then
            Assert.NotNull(result);
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            var executionResult = badResult.Value;
            Assert.Equal(expectedExecutionResult, executionResult);
        }
    }
}
