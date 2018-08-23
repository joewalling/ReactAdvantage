using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReactAdvantage.Api.GraphQLSchema;
using ReactAdvantage.Data;

namespace ReactAdvantage.Tests.Unit.Graphql
{
    public class GraphqlTestBase
    {
        private readonly ServiceProvider _serviceProvider;

        public GraphqlTestBase()
        {
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
                .UseInMemoryDatabase(databaseName: "ReactAdvantage")
                .Options;

            return new ReactAdvantageContext(options);
        }
    }
}
