using GraphQL.Types;
using ReactAdvantage.Data.EntityFramework.TypeModels;

namespace ReactAdvantage.Data.EntityFramework.GraphQlQuery
{
    public class GraphQlQuery : ObjectGraphType
    {
        private readonly ReactAdvantageContext _db;

        public GraphQlQuery(ReactAdvantageContext db)
        {
            _db = db;

            Field<ListGraphType<UserTypeModel>>(
                "Users",
                resolve: context => _db.Users
            );
        }
    }
}