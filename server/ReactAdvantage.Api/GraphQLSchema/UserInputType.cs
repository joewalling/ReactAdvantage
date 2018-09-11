using GraphQL.Types;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.GraphQLSchema
{
    public class UserInputType : InputObjectGraphType<User>
    {
        public UserInputType()
        {
            Name = "UserInput";
            Field(x => x.Id, nullable: true);
            Field(x => x.FirstName, nullable: true);
            Field(x => x.LastName, nullable: true);
            Field(x => x.UserName, nullable: true);
            Field(x => x.Email, nullable: true);
            Field(x => x.IsActive, nullable: true);
            Field(typeof(StringGraphType), "password");
        }
    }
}
