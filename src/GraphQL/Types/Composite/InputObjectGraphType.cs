using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace GraphQL.Types
{
    public interface IInputObjectGraphType : IComplexGraphType
    {

        /// <summary>
        /// Value input coercion. This transforms a dictionary of values from its client-side representation
        /// as a variable or literal to its server-side representation.
        /// </summary>
        /// <param name="value">Runtime dictionary parsed from variables or literal.</param>
        object Parse(IDictionary<string, object> value);
    }

    public class InputObjectGraphType : InputObjectGraphType<object>
    {
    }

    public class InputObjectGraphType<TSourceType> : ComplexGraphType<TSourceType>, IInputObjectGraphType
    {
        public virtual object Parse(IDictionary<string, object> value) => value;
    }

    public class InputObjectGraphType<TSourceType, TObjectType> : InputObjectGraphType<TSourceType>, IInputObjectGraphType
        where TObjectType : new()
    {
        public override object Parse(IDictionary<string, object> value)
            => value.ToObject(typeof(TObjectType), this);
    }
}

