using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

/// <summary>
/// Extension methods for System.Type
/// </summary>
public static class TypeExtension
{
    /// <summary>
    /// Returns True if the specified type is an AnonymousType.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Boolean IsAnonymousType(this Type type)
    {
        Boolean hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Count() > 0;
        Boolean nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
        Boolean isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

        return isAnonymousType;
    }
}

