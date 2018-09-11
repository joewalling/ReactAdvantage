using GraphQL.Types;

namespace ReactAdvantage.Api.GraphQLSchema
{
    public class ArgumentGetter<TArgument>
    {
        private readonly string _argumentName;
        private readonly ResolveFieldContext<object> _context;

        public ArgumentGetter(string argumentName, ResolveFieldContext<object> context)
        {
            _argumentName = argumentName;
            _context = context;
        }

        public bool HasArgument()
        {
            return _context.HasArgument(_argumentName);
        }

        public TArgument GetArgument()
        {
            return _context.GetArgument<TArgument>(_argumentName);
        }
    }
}
