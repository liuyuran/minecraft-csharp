using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Base.Utils {
    public static class TypeExtensions {
        public static Dictionary<Type, Type> GetTypesImplementingOpenGenericType(this Type openGenericType,
            Assembly assembly) {
            return assembly.GetTypes()
                .SelectMany(type => type.GetInterfaces(), (type, interfaceType) => new { type, interfaceType })
                .Select(@t => new { @t, baseType = @t.type.BaseType })
                .Where(@t =>
                    (@t.baseType is { IsGenericType: true } &&
                     openGenericType.IsAssignableFrom(@t.baseType.GetGenericTypeDefinition())) ||
                    (@t.@t.interfaceType.IsGenericType &&
                     openGenericType.IsAssignableFrom(@t.@t.interfaceType.GetGenericTypeDefinition())))
                .Select(@t => @t.t)
                .ToDictionary(t => t.type, t => t.interfaceType.GenericTypeArguments[0]);
        }
    }
}