using System;
using System.Linq;

namespace ReactAdvantage.Api.GraphQLSchema
{
    public static class GraphQLExtensions
    {
        public static IQueryable<TQuery> HandleQueryArgument<TQuery, TArgument>(this IQueryable<TQuery> query, ArgumentGetter<TArgument> argumentGetter, Func<TArgument, IQueryable<TQuery>, IQueryable<TQuery>> handler)
        {
            if (!argumentGetter.HasArgument())
            {
                return query;
            }

            var argumentValue = argumentGetter.GetArgument();

            return handler(argumentValue, query);
        }
    }
}
