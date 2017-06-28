using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Euroland.NetCore.ToolsFramework.Data
{
    public sealed class TypeUtils
    {
        public static bool IsPrimitive(Type type)
        {
            // Must use Type.GetTypeInfo() in .NetCore instead of Type
            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                typeInfo = type.GetGenericArguments()[0].GetTypeInfo();
            }

            return typeInfo.IsPrimitive
              || typeInfo.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal));
        }

        public static bool IsIEnumerable(Type type)
        {
            // Must use Type.GetTypeInfo() in .NetCore instead of 
            return typeof(System.Collections.IEnumerable).IsAssignableFrom(type);
        }
    }
}
