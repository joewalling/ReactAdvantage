using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.GraphQLSchema.Models
{
    public class UserInput : User
    {
        public string Password { get; set; }    
    }
}
