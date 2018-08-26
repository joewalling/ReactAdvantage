using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReactAdvantage.Api.GraphQLSchema;
using ReactAdvantage.Data;
using Xunit;

namespace ReactAdvantage.Tests.Unit.Api.Graphql
{
    public class GraphqlTestBase
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly string _databaseName;

        public GraphqlTestBase()
        {
            _databaseName = Guid.NewGuid().ToString();

            var services = new ServiceCollection();

            services.AddTransient<ReactAdvantageContext>(x => GetInMemoryDbContext());
            services.AddGraphqlServices();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected async Task<ExecutionResult> BuildSchemaAndExecuteQueryAsync(GraphQLQuery query)
        {
            var schema = _serviceProvider.GetService<ISchema>();
            var documentExecuter = _serviceProvider.GetService<IDocumentExecuter>();

            var executionOptions = new ExecutionOptions
            {
                Schema = schema,
                Query = query.Query,
                Inputs = query.Variables.ToInputs()
            };

            var result = await documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

            return result;
        }

        protected ReactAdvantageContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ReactAdvantageContext>()
                .UseInMemoryDatabase(databaseName: _databaseName)
                .Options;

            return new ReactAdvantageContext(options);
        }
        
        protected void AssertValidGraphqlExecutionResult(ExecutionResult result)
        {
            Assert.NotNull(result);
            Assert.False(result.Errors?.Count > 0, result.Errors?.Count > 0 ? "Graphql error: " + string.Join("; ", result.Errors.Select(x => x.Message)) : "");
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
