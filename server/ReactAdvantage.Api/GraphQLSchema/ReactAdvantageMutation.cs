using GraphQL.Types;
using ReactAdvantage.Data;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.GraphQLSchema
{
    public class ReactAdvantageMutation : ObjectGraphType
    {
        public ReactAdvantageMutation(ReactAdvantageContext db)
        {
            Field<UserType>(
                "addUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }
                ),
                resolve: context =>
                {
                    var user = context.GetArgument<User>("user");
                    user.Id = 0;
                    db.Add(user);
                    db.SaveChanges();
                    return user;
                });

            Field<UserType>(
                "editUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }
                ),
                resolve: context =>
                {
                    var user = context.GetArgument<User>("user");
                    var entity = db.Users.Find(user.Id);
                    db.Entry(entity).CurrentValues.SetValues(user);
                    db.SaveChanges();
                    return user;
                });
        }
    }
}
