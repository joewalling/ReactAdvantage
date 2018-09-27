using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Language.AST;
using Moq;
using Newtonsoft.Json.Linq;
using ReactAdvantage.Api.GraphQLSchema;
using Xunit;

namespace ReactAdvantage.Tests.Unit.Api.Graphql
{
    public class GraphQLQueryShould
    {
        [Fact]
        public void ConvertToString()
        {
            //Given
            var variables = new Mock<JObject>();
            variables.Setup(x => x.ToString()).Returns("Test Variables");

            var query = new GraphQLQuery
            {
                OperationName = "Test Operation",
                NamedQuery = "Test Named Query",
                Query = "Test Query",
                Variables = variables.Object
            };

            //When
            var str = query.ToString();

            //Then
            Assert.Equal(
@"
OperationName = Test Operation
NamedQuery = Test Named Query
Query = Test Query
Variables = Test Variables
", str);
        }

        [Fact]
        public void ConvertToEmptyString()
        {
            //Given
            var query = new GraphQLQuery
            {
            };

            //When
            var str = query.ToString();

            //Then
            Assert.Equal(Environment.NewLine, str);
        }
    }
}
