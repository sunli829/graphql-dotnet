using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace GraphQL.Types
{
    public interface IInputObjectGraphType : IComplexGraphType
    {
        object Parse(IDictionary<string, object> value);
    }

    public class InputObjectGraphType : InputObjectGraphType<object>
    {
    }

    public class InputObjectGraphType<TSourceType> : ComplexGraphType<TSourceType>, IInputObjectGraphType
    {
        public object Parse(IDictionary<string, object> value) => value;
    }

    public class InputObjectGraphType<TSourceType, TObjectType> : ComplexGraphType<TSourceType>, IInputObjectGraphType
        where TObjectType : new()
    {
        public virtual object Parse(IDictionary<string, object> value)
        {
            var newObj = new TObjectType();
            var tType = typeof(TObjectType);
            foreach (var field in Fields)
            {
                if (value.TryGetValue(field.Name, out var fieldValue))
                {
                    var objProp = tType.GetProperty(field.Name, BindingFlags.Public | BindingFlags.Instance);
                    if (objProp != null)
                    {
                        objProp.SetValue(newObj, fieldValue);
                    }
                    else
                    {
                        var objField = tType.GetField(field.Name, BindingFlags.Public | BindingFlags.Instance);
                        objField.SetValue(newObj, fieldValue);
                    }
                }
            }
            return newObj;
        }
    }
}

