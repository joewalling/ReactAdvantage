using GraphQL.Types;
using ReactAdvantage.Domain.Entities;

namespace ReactAdvantage.Data.EntityFramework.TypeModels
{
    /// <summary>
    ///     Graph ql type model
    /// </summary>
    public class UserTypeModel : ObjectGraphType<User>
    {
        public UserTypeModel()
        {
            Field(x => x.Id).Description("The Id of the User.");
            Field(x => x.Name, true).Description("The name of the User.");
            Field(x => x.FirstName, true).Description("The first name of user.");
            Field(x => x.LastName, true).Description("The LastName Of User");
            Field(x => x.Email, false).Description("The email of user.");
            Field(x => x.EmailConfirm, false).Description("Email Confirmation");
            Field(x => x.Roles, false).Description("Role of User");
            Field(x => x.Active, false).Description("Active user");
        }
    }

    public class List<UserTypeModel> : ObjectGraphType<User>
    {
    }
}