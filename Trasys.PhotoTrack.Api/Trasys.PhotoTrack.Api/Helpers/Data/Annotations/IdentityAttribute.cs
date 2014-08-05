using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Trasys.Dev.Tools.Data.Annotations
{
    /// <summary>
    /// Specifies the database Identity column.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class IdentityAttribute : Attribute
    {
        readonly bool _isIdentity;

        /// <summary>
        /// Initializes a new instance of the IdentityAttribute.
        /// </summary>
        public IdentityAttribute()
            : this(true)
        {

        }

        /// <summary>
        /// Initializes a new instance of the IdentityAttribute.
        /// </summary>
        /// <param name="isIdentity">True to set this property as a Identity Key.</param>
        public IdentityAttribute(bool isIdentity)
        {
            _isIdentity = isIdentity;
        }

        /// <summary>
        /// Gets True if this columns is included in the primary key.
        /// </summary>
        public bool IsIdentity
        {
            get
            {
                return _isIdentity;
            }
        }

        /// <summary>
        /// Returns the Identity attributes for the specified property.
        /// </summary>
        /// <param name="property">Property</param>
        /// <returns>Identity attributes or null if not found</returns>
        internal static IdentityAttribute GetIdentityAttribute(PropertyInfo property)
        {
            object[] customAttributes = property.GetCustomAttributes(typeof(IdentityAttribute), false);

            if (customAttributes != null && customAttributes.Length > 0)
            {
                return customAttributes.FirstOrDefault(a => a is IdentityAttribute) as IdentityAttribute;
            }

            return null;
        }
    }
}
