using System;
using System.Reflection;

namespace Moncore.CrossCutting.Extensions
{
    public static class TypeExtensions
    {
        // Example: typeof($).ToCamelCase()
        public static string ToCamelCase(this Type obj)
        {
            var name = obj.Name;
            return $"{name[0].ToString().ToLower()}{name.Substring(1, name.Length - 1)}";
        }

        // Example: typeof($).ToCamelCase()
        public static string ToCamelCase(this MemberInfo obj)
        {
            var name = obj.Name;
            return $"{name[0].ToString().ToLower()}{name.Substring(1, name.Length - 1)}";
        }
    }
}
