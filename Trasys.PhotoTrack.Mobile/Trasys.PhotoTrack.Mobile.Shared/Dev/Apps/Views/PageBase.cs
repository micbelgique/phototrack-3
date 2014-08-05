using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trasys.Dev.Tools.Apps;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Trasys.Dev.Views.Apps
{
    /// <summary>
    /// Window Apps Page (RT or Phone) with default properties
    /// </summary>
    /// <remarks>
    /// To use this PageBase class:
    /// <list type="bullet">
    /// <item>
    /// Creates your ViewModelLocator class which inherits ViewModelLocatorBase (in Trasys.Dev.Tools.Mvvm namespace),
    /// where you will add all your ViewModels objets.
    /// </item>
    /// <code>
    ///     public class ViewModelLocator : Trasys.Dev.Tools.Mvvm.ViewModelLocatorBase<IFactory>
    ///     {
    ///         public ViewModelLocator(IFactory factory)
    ///         {
    ///             this.Factory = factory;
    ///             this.MyReports = new ReportsViewModel(factory);            
    ///         }
    /// 
    ///         public ReportsViewModel MyReports { get; set; }
    ///     }
    /// </code>
    /// <item>
    /// Next, creates a ViewModelLocator property into App class (App.Xaml.cs)
    /// <code>
    ///    private ViewModelLocator _viewModelLocator = null;
    ///    public ViewModelLocator ViewModelLocator
    ///    {
    ///        get
    ///        {
    ///            if (_viewModelLocator == null) _viewModelLocator = new ViewModelLocator(new Factory());
    ///            return _viewModelLocator;
    ///        }
    ///    }
    /// </code>
    /// </item>
    /// <item>
    /// Next, creates a PageBase class which inherits this abstract class to define your ViewModelLocator property.
    /// <code>
    ///    public class PageBase : Trasys.Dev.Views.Apps.PageBase
    ///    {
    ///        public override ViewModelLocator ViewModelLocator
    ///        {
    ///            get
    ///            {
    ///                return (global::Windows.UI.Xaml.Application.Current as MySoftware.App).ViewModelLocator;
    ///            }
    ///        }
    ///        
    ///    }
    /// </code>
    /// </item>
    /// <item>
    /// Finaly, inherits all UI pages from this new PageBase class, 
    /// by renaming &lt;Page&gt; tag in your XAML files to &lt;local:PageBase&gt;
    /// and renaming the associated base class in the code behind to PageBase.
    /// So, you can use this.ViewModelLocator in each page to find your ViewModel.
    /// And you can use this.Factory in each ViewModel to find your data.
    /// <code>
    ///     public sealed partial class MainPage : PageBase
    ///     {
    ///         ...
    ///     }
    /// </code>
    /// </item>
    /// </list>
    /// </remarks>
    public abstract class PageBase<TViewModelLocator> : Page
    {
        private readonly NavigationHelper navigationHelper;
#if !WINDOWS_APP
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
#endif

        /// <summary>
        /// Initializes a new instance of PageBase
        /// </summary>
        public PageBase()
        {
            this.navigationHelper = new NavigationHelper(this);            
        }

#if !WINDOWS_APP
        /// <summary>
        /// Obsolete property. See <see cref="ViewModelLocator"/>
        /// Gets a reference to the Business Factory
        /// </summary>
        [Obsolete()]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public virtual TViewModelLocator Factory { get { return this.ViewModelLocator; } }
#endif

        /// <summary>
        /// Gets a reference to the ViewModel
        /// </summary>
        public virtual TViewModelLocator ViewModelLocator { get; private set; }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public virtual NavigationHelper Navigation
        {
            get { return this.navigationHelper; }
        }

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

#if !WINDOWS_APP
        /// <summary>
        /// Obsolete property. See <see cref="ViewModelLocator"/>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        [Obsolete()]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
#endif
    }



}

