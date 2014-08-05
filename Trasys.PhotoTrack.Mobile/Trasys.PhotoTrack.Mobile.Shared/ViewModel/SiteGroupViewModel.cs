using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Trasys.Dev.Tools.Mvvm;

namespace Trasys.PhotoTrack.Mobile.ViewModel
{

    /// <summary>
    /// Group of Site
    /// </summary>
    public class SiteGroupViewModel : ViewModelBase<Model.Factory>
    {
        #region DECLARATIONS

        private ViewModelLocator _locator = null;
        private SiteItemViewModelCollection _items = new SiteItemViewModelCollection();

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Initializes a new empty instance, for Sample Data.
        /// </summary>
        public SiteGroupViewModel()
        {

        }

        // <summary>
        /// Initializes a new empty instance.
        /// </summary>
        public SiteGroupViewModel(ViewModelLocator locator, Model.Factory factory)
        {
            this._locator = locator;
            this.Factory = factory;
        }

        // <summary>
        /// Initializes a new empty instance.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="groupName"></param>
        public SiteGroupViewModel(ViewModelLocator locator, Model.Factory factory, string categoryName)
        {
            this._locator = locator;
            this.Factory = factory;
            this.CategoryName = categoryName;
        }

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Gets a list with all items of this group.
        /// </summary>
        public SiteItemViewModelCollection Items
        {
            get
            {
                return this._items;
            }
        }

        /// <summary>
        /// Gets or sets the category name of this group.
        /// </summary>
        public string CategoryName { get; set; }

        #endregion
    }

    /// <summary>
    /// List of groups.
    /// </summary>
    public class SiteGroupViewModelCollection : ObservableCollection<SiteGroupViewModel>
    {
        
    }

    /// <summary>
    /// Types of Groups
    /// </summary>
    public enum SiteGroupViewModelType
    { 
        Favorites = 0,
        Delayed = 1,
        Actives = 2,
        News = 3,
        Closed = 4
    }
}
