using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Trasys.Dev.Tools.Mvvm;
using System.Runtime.Serialization;

namespace Trasys.Dev.Tools.Data.Entity
{
    /// <summary>
    /// Base class used to trace all property changed and to insert or update values to the database.
    /// </summary>
    [DataContract]
    public abstract class DbTable : NotifyPropertyChanged
    {
        private List<string> _propertyChanged = new List<string>();     // List of Property names where value has changed.

        /// <summary>
        /// Initializes a new instance of DbTable
        /// </summary>
        public DbTable()
        {
            this.ActivatesPropertyChangedEvent(true);
        }

        /// <summary>
        /// Activates or disactivates the PropertyChanged event to keep trace of modifications. 
        /// </summary>
        /// <param name="isActive"></param>
        internal virtual void ActivatesPropertyChangedEvent(bool isActive)
        { 
            if (isActive)
                this.PropertyChanged += DbTable_PropertyChanged;
            else
                this.PropertyChanged -= DbTable_PropertyChanged;
        }

        /// <summary>
        /// Clear all property changes saved.
        /// </summary>
        public virtual void ClearPropertyChanges()
        {
            if (_propertyChanged == null)
            {
                _propertyChanged = new List<string>();
            }
            _propertyChanged.Clear();
        }

        /// <summary>
        /// Returns True if the specified property name has changed.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsPropertyChanged(string name)
        {
            if (_propertyChanged == null)
            {
                _propertyChanged = new List<string>();
            }
            return _propertyChanged.Contains(name);
        }

        /// <summary>
        /// Raises after a property value has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DbTable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_propertyChanged == null)
            {
                _propertyChanged = new List<string>();
            }
            if (!_propertyChanged.Contains(e.PropertyName))
            {
                _propertyChanged.Add(e.PropertyName);
            }
        }


    }
}
