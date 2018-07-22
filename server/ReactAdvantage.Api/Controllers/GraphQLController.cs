using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using ReactAdvantage.Api.GraphQLSchema;

namespace ReactAdvantage.Api.Controllers
{
    [Produces("application/json")]
    [Route("graphql")]
    public class GraphQLController : ControllerBase
    {
        private ReactAdvantageQuery _reactAdvantageQuery { get; set; }

        public GraphQLController(ReactAdvantageQuery reactAdvantageQuery)
        {
            _reactAdvantageQuery = reactAdvantageQuery;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            var schema = new Schema { Query = _reactAdvantageQuery };

            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.Query = query.Query;

            }).ConfigureAwait(false);

            if (result.Errors?.Count > 0)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}