using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Trasys.Dev.Tools.Data.Annotations
{
    /// <summary>
    /// Specifies that the property has to be ignored in the mapping
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NotMappedAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the NotMapped.
        /// </summary>
        public NotMappedAttribute()
        {
        }
       
        /// <summary>
        /// Returns the NotMapped attributes for the specified property.
        /// </summary>
        /// <param name="property">Property</param>
        /// <returns>NotMapped attributes or null if not found</returns>
        internal static NotMappedAttribute GetNotMappedAttribute(PropertyInfo property)
        {
            object[] customAttributes = property.GetCustomAttributes(typeof(NotMappedAttribute), false);

            if (customAttributes != null && customAttributes.Length > 0)
            {
                return customAttributes.FirstOrDefault(a => a is NotMappedAttribute) as NotMappedAttribute;
            }

            return null;
        }
    }

}
