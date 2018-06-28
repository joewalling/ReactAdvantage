using System;
using System.Threading.Tasks;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using ReactAdvantage.Data.EntityFramework;
using ReactAdvantage.Data.EntityFramework.GraphQlQuery;
using ReactAdvantage.Data.EntityFramework.GraphQlQueryModel;


namespace ReactAdvantage.API.Controllers
{
    [Produces("application/json")]
    public class GraphQlController : Controller
    {
        private GraphQlQuery _graphQlQuery { get; set; }
      
        private readonly ReactAdvantageContext _context;

        public GraphQlController(GraphQlQuery graphQLQuery, ReactAdvantageContext context )
        {
            _context = context;
            _graphQlQuery = graphQLQuery;
           
        }

        /// <summary>
        /// Get users from database  using graph ql
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>

        [Route("api/GetUsers")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQlQueryModel query)
        {
            try {
                var schema = new Schema { Query = new GraphQlQuery(_context) };
                var result = await new GraphQL.DocumentExecuter().ExecuteAsync(_ =>
                {
                    _.Schema = schema;
                    _.Query = query.Query;

                }).ConfigureAwait(false);
                if (result.Errors != null)
                {
                    if (result.Errors.Count > 0)
                    {
                        return BadRequest();
                    }
                }

                return Ok(result);

            }catch(Exception e)
            {
                throw e;
            }
            }

    
    }
    
}
