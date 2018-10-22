using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Identity;
using ReactAdvantage.Domain.MultiTenancy;

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

        public static GraphQLUserContext GetUserContext<T>(this ResolveFieldContext<T> context)
        {
            return context.UserContext as GraphQLUserContext;
        }

        public static void SetTenantIdOrThrow<T>(this IMustHaveTenant entity, ResolveFieldContext<T> context)
        {
            var userContext = context.GetUserContext();
            if (userContext.TenantId == null)
            {
                throw new ExecutionError("No tenant id. You have to be a member of a tenant to be able to add or edit this record");
            }

            entity.TenantId = userContext.TenantId.Value;
        }

        public static void SetTenantIdOrThrow<T>(this IMayHaveTenant entity, ResolveFieldContext<T> context)
        {
            var userContext = context.GetUserContext();
            entity.TenantId = userContext.TenantId;
        }
    }
}
