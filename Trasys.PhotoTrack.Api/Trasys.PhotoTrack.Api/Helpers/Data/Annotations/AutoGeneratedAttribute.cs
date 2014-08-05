﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Trasys.Dev.Tools.Data.Annotations
{
    /// <summary>
    /// Specifies the database AutoGenerated column.
    /// If not specified, the default value is IdentityAttribute or false if not set.
    /// This columns will not inserted or updated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class AutoGeneratedAttribute : Attribute
    {
        readonly bool _isAutoGenerated;

        /// <summary>
        /// Initializes a new instance of the AutoGeneratedAttribute.
        /// </summary>
        public AutoGeneratedAttribute()
            : this(true)
        {

        }

        /// <summary>
        /// Initializes a new instance of the AutoGeneratedAttribute.
        /// </summary>
        /// <param name="isAutoGenerated">True to set this property as an AutoGenerated Column: this columns will not inserted or updated.</param>
        public AutoGeneratedAttribute(bool isAutoGenerated)
        {
            _isAutoGenerated = isAutoGenerated;
        }

        /// <summary>
        /// Gets True if this columns is an AutoGenerated Column.
        /// </summary>
        public bool IsAutoGenerated
        {
            get
            {
                return _isAutoGenerated;
            }
        }

        /// <summary>
        /// Returns the AutoGenerated attributes for the specified property.
        /// </summary>
        /// <param name="property">Property</param>
        /// <returns>AutoGenerated attributes or null if not found</returns>
        internal static AutoGeneratedAttribute GetAutoGeneratedAttribute(PropertyInfo property)
        {
            object[] customAttributes = property.GetCustomAttributes(typeof(AutoGeneratedAttribute), false);

            if (customAttributes != null && customAttributes.Length > 0)
            {
                return customAttributes.FirstOrDefault(a => a is AutoGeneratedAttribute) as AutoGeneratedAttribute;
            }

            return null;
        }
    }
}
