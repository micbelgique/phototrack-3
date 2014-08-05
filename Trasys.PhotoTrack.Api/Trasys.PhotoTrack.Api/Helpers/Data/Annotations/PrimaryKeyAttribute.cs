using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Trasys.Dev.Tools.Data.Annotations
{
    /// <summary>
    /// Specifies the database Primary Key column.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class PrimaryKeyAttribute : Attribute
    {
        readonly bool _isPrimaryKey;

        /// <summary>
        /// Initializes a new instance of the PrimaryKeyAttribute.
        /// </summary>
        public PrimaryKeyAttribute() : this(true)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the PrimaryKeyAttribute.
        /// </summary>
        /// <param name="isPrimaryKey">True to set this property as a Primary Key.</param>
        public PrimaryKeyAttribute(bool isPrimaryKey)
        {
            _isPrimaryKey = isPrimaryKey;
        }

        /// <summary>
        /// Gets True if this columns is included in the primary key.
        /// </summary>
        public bool IsPrimaryKey
        {
            get
            {
                return _isPrimaryKey;
            }
        }  

        /// <summary>
        /// Returns the PrimaryKey attributes for the specified property.
        /// </summary>
        /// <param name="property">Property</param>
        /// <returns>PrimaryKey attributes or null if not found</returns>
        internal static PrimaryKeyAttribute GetPrimaryKeyAttribute(PropertyInfo property)
        {
            object[] customAttributes = property.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);

            if (customAttributes != null && customAttributes.Length > 0)
            {
                return customAttributes.FirstOrDefault(a => a is PrimaryKeyAttribute) as PrimaryKeyAttribute;
            }

            return null;
        }

    }
}
