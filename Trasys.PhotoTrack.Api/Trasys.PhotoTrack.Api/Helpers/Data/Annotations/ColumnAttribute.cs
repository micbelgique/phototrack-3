using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Trasys.Dev.Tools.Data.Annotations
{
    /// <summary>
    /// Specifies the database column that a property is mapped to.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ColumnAttribute : Attribute
    {
        readonly string _name;

        /// <summary>
        /// Initializes a new instance of the ColumnAttribute.
        /// </summary>
        /// <param name="name">The name of the column the property is mapped to.</param>
        public ColumnAttribute(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Gets the name of the column the property is mapped to.
        /// </summary>
        public string Name
        {
            get 
            {
                return _name; 
            }
        }

        /// <summary>
        /// Returns the Column attributes for the specified property.
        /// </summary>
        /// <param name="property">Property</param>
        /// <returns>Column attributes or null if not found</returns>
        internal static ColumnAttribute GetColumnAttribute(PropertyInfo property)
        {
            object[] customAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), false);

            if (customAttributes != null && customAttributes.Length > 0)
            {
                return customAttributes.FirstOrDefault(a => a is ColumnAttribute) as ColumnAttribute;
            }

            return null;
        }
    }

}
