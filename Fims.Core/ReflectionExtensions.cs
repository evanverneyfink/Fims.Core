using System;
using System.Collections;
using System.Collections.Generic;

namespace Fims.Core
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Checks if a type can be assigned to an IEnumerable of a given item type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsAssignableToEnumerableOf<T>(this Type type)
        {
            return type.IsGenericType &&
                   type.GenericTypeArguments.Length == 1 &&
                   typeof(T).IsAssignableFrom(type.GenericTypeArguments[0]) &&
                   typeof(IEnumerable<>).MakeGenericType(type.GenericTypeArguments[0])
                                        .IsAssignableFrom(type);
        }

        /// <summary>
        /// Checks if a type is a generic enumerable
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsGenericEnumerable(this Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) && type.IsGenericType && type.GenericTypeArguments.Length == 1;
        }
    }
}