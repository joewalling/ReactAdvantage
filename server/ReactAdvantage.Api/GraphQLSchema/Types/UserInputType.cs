using GraphQL.Types;
using ReactAdvantage.Api.GraphQLSchema.Models;

namespace ReactAdvantage.Api.GraphQLSchema.Types
{
    public class UserInputType : InputObjectGraphType<UserInput>
    {
        public UserInputType()
        {
            Name = "UserInput";
            Field(x => x.Id, nullable: true);
            Field(x => x.FirstName, nullable: true);
            Field(x => x.LastName, nullable: true);
            Field(x => x.UserName, nullable: false);
            Field(x => x.Email, nullable: true);
            Field(x => x.IsActive, nullable: true);
            Field(x => x.Password, nullable: true);
            Field(x => x.Roles, nullable: true);
        }
    }
}
