using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace Trasys.Dev.Tools.Mvvm
{
#if NETFX_45
    /// <summary>
    /// Notifies clients that a property value has changed.
    /// </summary>
    /// <remarks>
    /// <code>
    /// private int _myProperty = 0;
    /// public int MyProperty
    /// {
    ///   get { return _myProperty; }
    ///   set { SetProperty(ref _myProperty, value); }
    /// }
    /// </code>
    /// </remarks>
#else
    /// <summary>
    /// Notifies clients that a property value has changed.
    /// </summary>
    /// <remarks>
    /// <remarks>
    /// <code>
    /// private int _myProperty = 0;
    /// public int MyProperty
    /// {
    ///   get { return _myProperty; }
    ///   set { SetProperty(ref _myProperty, value, "MyProperty"); }
    /// }
    /// </code>
    /// </remarks>
#endif

    [DataContract]
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region EVENTS

        /// <summary>
        /// Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Multicast event for property changing notifications (before property changed).
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;
                
        #endregion

        #region METHODS

        /// <summary>
        /// Returns True if the specified PropertyChangedEventHandler is already registered.
        /// </summary>
        /// <param name="prospectiveEventHandler"></param>
        /// <returns></returns>
        public virtual bool IsPropertyChangedRegistered(PropertyChangedEventHandler prospectiveEventHandler)
        {
            if (PropertyChanged != null)
            {                
                return PropertyChanged.GetInvocationList().Contains(prospectiveEventHandler);
            }
            
            return false;
        }

        /// <summary>
        /// Returns True if the specified PropertyChangingEventHandler is already registered.
        /// </summary>
        /// <param name="prospectiveEventHandler"></param>
        /// <returns></returns>
        public virtual bool IsPropertyChangingRegistered(PropertyChangedEventHandler prospectiveEventHandler)
        {
            if (PropertyChanging != null)
            {
                return PropertyChanging.GetInvocationList().Contains(prospectiveEventHandler);
            }

            return false;
        }

        /// <summary>
        /// Raises the PropertyChanged event (after the property value was changed).
        /// </summary>
        /// <param name="e">Arguments to define the property name</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the PropertyChanging event (before the property value will be changed).
        /// </summary>
        /// <param name="e">Arguments to define the property name</param>
        protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, e);
            }
        }

#if NETFX_45

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Notifies listeners that a property value will be change.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Checks if a property already matches a desired value.  Sets the property and
        /// notifies listeners only when necessary.
        /// <example>
        /// <code>
        /// private int _myProperty = 0;
        /// public int MyProperty
        /// {
        ///   get { return _myProperty; }
        ///   set { SetProperty(ref _myProperty, value); }
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="field">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(field, value)) return false;

            this.OnPropertyChanging(propertyName);
            field = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Checks if a property already matches a desired value.  Sets the property and
        /// notifies listeners only when necessary.
        /// <example>
        /// <code>
        /// private int _myProperty = 0;
        /// public int MyProperty
        /// {
        ///   get { return _myProperty; }
        ///   set 
        ///   { 
        ///      SetProperty(ref _myProperty, value, (o, n) =>
        ///      {
        ///         Debug.Print(String.Format("Old Value = {0}; New Value = {1}.", o, n));
        ///      }); 
        ///   }
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="field">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="action">Action to execute after changing the property value.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        protected virtual bool SetProperty<T>(ref T field, T value, Action<T, T> action, [CallerMemberName] string propertyName = "")
        {
            if (object.Equals(field, value)) return false;

            this.OnPropertyChanging(propertyName);
            T oldValue = field;
            field = value;
            this.OnPropertyChanged(propertyName);
            action.Invoke(oldValue, value);
            return true;
        }

#else

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Notifies listeners that a property value will be change.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        protected virtual void OnPropertyChanging(string propertyName)
        {
            OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Checks if a property already matches a desired value.  Sets the property and
        /// notifies listeners only when necessary.
        /// <example>
        /// <code>
        /// private int _myProperty = 0;
        /// public int MyProperty
        /// {
        ///   get { return _myProperty; }
        ///   set { SetProperty(ref _myProperty, value, "MyProperty"); }
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="field">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        protected virtual bool SetProperty<T>(ref T field, T value, string propertyName)
        {
            if (object.Equals(field, value)) return false;

            this.OnPropertyChanging(propertyName);
            field = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Checks if a property already matches a desired value.  Sets the property and
        /// notifies listeners only when necessary.
        /// <example>
        /// <code>
        /// private int _myProperty = 0;
        /// public int MyProperty
        /// {
        ///   get { return _myProperty; }
        ///   set 
        ///   { 
        ///      SetProperty(ref _myProperty, value, "MyProperty", (o, n) =>
        ///      {
        ///         Debug.Print(String.Format("Old Value = {0}; New Value = {1}.", o, n));
        ///      }); 
        ///   }
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="field">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="action">Action to execute after changing the property value.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        protected virtual bool SetProperty<T>(ref T field, T value, string propertyName, Action<T, T> action)
        {
            if (object.Equals(field, value)) return false;

            this.OnPropertyChanging(propertyName);
            T oldValue = field;
            field = value;
            this.OnPropertyChanged(propertyName);
            action.Invoke(oldValue, value);
            return true;
        }

#endif

        #endregion

    }

}

#if NETFX_45

namespace System.ComponentModel
{
    public delegate void PropertyChangingEventHandler(object sender, PropertyChangingEventArgs e);

    // Summary:
    //     Notifies clients that a property value is changing.
    public interface INotifyPropertyChanging
    {
        // Summary:
        //     Occurs when a property value is changing.
        event PropertyChangingEventHandler PropertyChanging;
    }

    // Summary:
    //     Provides data for the System.ComponentModel.INotifyPropertyChanging.PropertyChanging
    //     event.
    public class PropertyChangingEventArgs : EventArgs
    {
        // Summary:
        //     Initializes a new instance of the System.ComponentModel.PropertyChangingEventArgs
        //     class.
        //
        // Parameters:
        //   propertyName:
        //     The name of the property whose value is changing.
        public PropertyChangingEventArgs(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        // Summary:
        //     Gets the name of the property whose value is changing.
        //
        // Returns:
        //     The name of the property whose value is changing.
        public virtual string PropertyName { get; private set; }
    }
}

#endif