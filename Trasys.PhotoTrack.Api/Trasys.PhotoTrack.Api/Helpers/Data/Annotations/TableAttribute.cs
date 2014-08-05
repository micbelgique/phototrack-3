using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Trasys.Dev.Tools.Data.Annotations
{
    /// <summary>
    /// Specifies the database table name mapped to the class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class TableAttribute : Attribute
    {
        readonly string _name;

        /// <summary>
        /// Initializes a new instance of the TableAttribute.
        /// </summary>
        /// <param name="name">The name of the table the class is mapped to.</param>
        public TableAttribute(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Gets the name of the table the class is mapped to.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Returns the Table attributes for the specified object
        /// </summary>
        /// <param name="value">object</param>
        /// <returns>Table attributes or null if not found</returns>
        internal static TableAttribute GetTableAttribute(object value)
        {
            return GetTableAttribute(value.GetType());
        }

        /// <summary>
        /// Returns the Table attributes for the specified property and for the type specified
        /// </summary>
        /// <returns>Table attributes or null if not found</returns>
        /// <param name="type">Type of the object</param>
        internal static TableAttribute GetTableAttribute(Type type)
        {
            object[] customAttributes = type.GetCustomAttributes(typeof(TableAttribute), false);

            if (customAttributes != null && customAttributes.Length > 0)
            {
                return customAttributes.FirstOrDefault(a => a is TableAttribute) as TableAttribute;
            }

            return null;
        }
    }
}
