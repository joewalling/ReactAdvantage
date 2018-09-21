using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL;
using Microsoft.AspNetCore.Identity;

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

        public static Inputs ToInputs(this Dictionary<string, object> dictionary)
        {
            return new Inputs(dictionary ?? new Dictionary<string, object>());
        }

        public static void ThrowOnError(this IdentityResult identityResult)
        {
            if (identityResult.Succeeded)
            {
                return;
            }

            var error = identityResult.Errors.FirstOrDefault();
            var errorMessage = error != null ? error.Code + ": " + error.Description : "Identity error";
            throw new ExecutionError(errorMessage);
        }
    }
}
