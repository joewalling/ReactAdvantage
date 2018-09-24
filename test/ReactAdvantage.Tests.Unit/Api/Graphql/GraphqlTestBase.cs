using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using ReactAdvantage.Api.Extensions;
using ReactAdvantage.Api.GraphQLSchema;
using ReactAdvantage.Data;
using ReactAdvantage.Domain.Models;
using Xunit;

namespace ReactAdvantage.Tests.Unit.Api.Graphql
{
    public class GraphqlTestBase
    {
        private readonly string _databaseName;
        protected ServiceProvider ServiceProvider { get; }

        public GraphqlTestBase()
        {
            _databaseName = Guid.NewGuid().ToString();

            var services = new ServiceCollection();

            services.AddTransient<ReactAdvantageContext>(x => GetInMemoryDbContext());
            services.AddIdentityCore<User, IdentityRole, ReactAdvantageContext>();
            services.AddGraphqlServices();

            ServiceProvider = services.BuildServiceProvider();
        }

        protected async Task<ExecutionResult> BuildSchemaAndExecuteQueryAsync(GraphQLQuery query, GraphQLUserContext userContext = null)
        {
            var schema = ServiceProvider.GetService<ISchema>();
            var documentExecuter = ServiceProvider.GetService<IDocumentExecuter>();

            var executionOptions = new ExecutionOptions
            {
                Schema = schema,
                Query = query.Query,
                Inputs = query.Variables.ToInputs(),
                UserContext = userContext
            };

            var result = await documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

            return result;
        }

        protected ReactAdvantageContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ReactAdvantageContext>()
                .UseInMemoryDatabase(databaseName: _databaseName)
                .Options;

            var dbLogger = new Mock<ILogger<ReactAdvantageContext>>();

            return new ReactAdvantageContext(options, dbLogger.Object);
        }
        
        protected void AssertValidGraphqlExecutionResult(ExecutionResult result)
        {
            Assert.NotNull(result);
            Assert.False(result.Errors?.Count > 0, result.Errors?.Count > 0 ? "GraphQL error: " + string.Join("; ", result.Errors.Select(x => x.Message)) : "");
            Assert.NotNull(result.Data);
        }

        protected void AssertGraphqlResultDictionary(object dataObject, params Action<KeyValuePair<string, object>>[] elementInspectors)
        {
            var data = Assert.IsType<Dictionary<string, object>>(dataObject);
            Assert.Collection(data, elementInspectors);
        }

        protected void AssertGraphqlResultArray(object collectionObject, params Action<Dictionary<string, object>>[] elementInspectors)
        {
            AssertGraphqlResultArray<Dictionary<string, object>>(collectionObject, elementInspectors);
        }

        protected void AssertGraphqlResultArray<TArrayElement>(object collectionObject, params Action<TArrayElement>[] elementInspectors)
        {
            var collectionOfObjects = Assert.IsType<object[]>(collectionObject);

            var collection = new List<TArrayElement>();
            foreach (var elementObject in collectionOfObjects)
            {
                var element = Assert.IsType<TArrayElement>(elementObject);
                collection.Add(element);
            }

            Assert.Collection(collection, elementInspectors);
        }

        protected void AssertPairEqual<TValue>(KeyValuePair<string, object> field, string fieldName, TValue fieldValue)
        {
            Assert.Equal(fieldName, field.Key);
            Assert.Equal(fieldValue, field.Value);
        }

        protected void AssertPairEqual(KeyValuePair<string, object> field, string fieldName, Action<object> valueInspector)
        {
            Assert.Equal(fieldName, field.Key);
            valueInspector = valueInspector ?? Assert.Null;
            valueInspector(field.Value);
        }
    }
}
