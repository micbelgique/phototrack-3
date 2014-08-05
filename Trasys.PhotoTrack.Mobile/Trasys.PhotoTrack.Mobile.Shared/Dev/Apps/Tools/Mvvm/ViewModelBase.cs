using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Trasys.Dev.Tools.Mvvm
{ 

#if NETFX_45
    /// <summary>
    /// Base class to define a MVVM ViewModel with a Factory property and Property notifications.
    /// </summary>
    /// <typeparam name="TFactory">Type of business (factory) class.</typeparam>
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
#else
    /// <summary>
    /// Base class to define a MVVM ViewModel with a Factory property and Property notifications.
    /// </summary>
    /// <typeparam name="TFactory">Type of business (factory) class.</typeparam>
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
#endif    
    public abstract class ViewModelBase<TFactory> : NotifyPropertyChanged
    {
        #region DECLARATIONS

        private bool _isWorking;

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Initializes a new instance of ViewModelBase
        /// </summary>
        protected ViewModelBase()
        {

        }

        /// <summary>
        /// Initializes a new instance of ViewModelBase
        /// </summary>
        /// <param name="factory">Reference to Business Factory</param>
        protected ViewModelBase(TFactory factory)
            : this()
        {
            this.Factory = factory;
        }

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Gets or sets a reference to the Business Factory.
        /// </summary>
        protected virtual TFactory Factory { get; set; }

        /// <summary>
        /// Gets or sets True if a ViewModel method is running. 
        /// For example, to display a waiting bar in asynchronous mode.
        /// </summary>
        public virtual bool IsWorking
        {
            get { return _isWorking; }
            set { this.SetProperty(ref _isWorking, value, "IsWorking"); }
        }
        
        #endregion

        #region METHODS

#if NETFX_45

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        protected override bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(field, value)) return false;

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        protected override bool SetProperty<T>(ref T field, T value, Action<T, T> action, [CallerMemberName] string propertyName = "")
        {
            if (object.Equals(field, value)) return false;

            T oldValue = field;
            field = value;
            this.OnPropertyChanged(propertyName);
            action.Invoke(oldValue, value);
            return true;
        }

        /// <summary>
        /// Schedules the provided callback on the UI thread from a worker thread, and returns the results asynchronously.
        /// </summary>
        /// <param name="action">Lambda expression to execute.</param>
        /// <returns></returns>
        public virtual async Task RunAsync(Action action)
        {
            if (this.IsDesignMode)
            {
                action.Invoke();
            }
            else
            {
                await global::Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(new global::Windows.UI.Core.CoreDispatcherPriority(), () => { action.Invoke(); });
            }
        }

        /// TODO: To be copied to Trasys.Dev
        /// --------------------------------
        /// <summary>
        /// Gets a value that indicates whether the process is running in design mode. 
        /// </summary>
        public virtual bool IsDesignMode
        {
            get
            {
                return global::Windows.ApplicationModel.DesignMode.DesignModeEnabled;
            }
        }     
#else

#endif

        #endregion

    }
}
