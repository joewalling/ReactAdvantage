using System.Collections.Generic;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.GraphQLSchema.Models
{
    public class UserInput : User
    {
        public string Password { get; set; }
        public List<string> Roles { get; set; }
    }
}
