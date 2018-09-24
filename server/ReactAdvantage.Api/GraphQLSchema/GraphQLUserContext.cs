using System.Linq;
using System.Security.Claims;
using GraphQL;

namespace ReactAdvantage.Api.GraphQLSchema
{
    public class GraphQLUserContext
    {
        public GraphQLUserContext(ClaimsPrincipal user)
        {
            User = user;
        }

        public ClaimsPrincipal User { get; set; }
        
        public string Id => User?.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

        public bool IsInRole(string role)
        {
            return User?.IsInRole(role) ?? false;
        }

        public void EnsureIsInRole(string role)
        {
            if (!IsInRole(role))
            {
                throw new ExecutionError($"Unauthorized. You have to be a member of {role} role.");
            }
        }
    }
}
