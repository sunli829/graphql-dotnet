using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Conversion;
using GraphQL.Types;
using Xunit;

namespace GraphQL.Tests.Bugs
{
    // https://github.com/graphql-dotnet/graphql-dotnet/issues/1952
    public class Issue1952 : QueryTestBase<Issue1952Schema>
    {
        [Fact]
        public void Issue1952_Custom_Built_In_Scalar()
        {
            var query = @"
query {
  test
}
";
            var expected = @"{
  ""test"": ""HELLO""
}";
            AssertQuerySuccess(query, expected, null);
        }
    }

    public class Issue1952Schema : Schema
    {
        public Issue1952Schema()
        {
            Query = new Issue1952Query();
        }

        protected override GraphTypesLookup CreateTypesLookup(IEnumerable<IGraphType> types, IEnumerable<DirectiveGraphType> directives, Func<Type, IGraphType> graphTypeResolver)
        {
            var lookup = new Issue1952GraphTypesLookup(NameConverter);

            lookup.Initialize(
                types,
                directives,
                graphTypeResolver,
                seal: true);

            return lookup;
        }

        private class Issue1952GraphTypesLookup : GraphTypesLookup
        {
            public Issue1952GraphTypesLookup(INameConverter nameConverter) : base(nameConverter)
            {
            }

            private IReadOnlyDictionary<Type, IGraphType> _builtInScalars;

            protected override IReadOnlyDictionary<Type, IGraphType> BuiltInScalars => _builtInScalars ??= GetBuiltInScalars();

            private IReadOnlyDictionary<Type, IGraphType> GetBuiltInScalars()
            {
                // Standard scalars https://graphql.github.io/graphql-spec/June2018/#sec-Scalars
                var builtInScalars = new IGraphType[]
                {
                    new BooleanGraphType(),
                    new FloatGraphType(),
                    new IntGraphType(),
                    new IdGraphType(),
                }
                .ToDictionary(t => t.GetType());

                builtInScalars.Add(typeof(StringGraphType), new Issue1952CustomStringGraphType());

                return builtInScalars;
            }
        }

        [GraphQLMetadata("String")]
        private class Issue1952CustomStringGraphType : StringGraphType
        {
            public override object Serialize(object value)
            {
                return value?.ToString().ToUpper();
            }
        }

        private class Issue1952Query : ObjectGraphType
        {
            public Issue1952Query()
            {
                Field<StringGraphType>(
                    "test",
                    resolve: ctx => "hello");
            }
        }
    }
}
