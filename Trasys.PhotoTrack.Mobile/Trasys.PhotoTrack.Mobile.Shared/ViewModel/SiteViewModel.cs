using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Trasys.Dev.Tools.Mvvm;
using Trasys.PhotoTrack.Mobile.Model.Entities;
using Trasys.PhotoTrack.Mobile.Model;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Trasys.PhotoTrack.Mobile.ViewModel
{
    /// <summary>
    /// Manage the Site View Model.
    /// </summary>
    public class SiteViewModel : ViewModelBase<Model.Factory>
    {
        #region DECLARATIONS

        private SiteGroupViewModelCollection _groups = new SiteGroupViewModelCollection();
        private ViewModelLocator _locator = null;

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Initializes a new empty instance, for Sample Data.
        /// </summary>
        public SiteViewModel()
        {
            this.SearchItemsFound = new ObservableCollection<SiteItemViewModel>();
        }

        /// <summary>
        /// Initializes a new instance of this ViewModel.
        /// </summary>
        public SiteViewModel(ViewModelLocator locator, Model.Factory factory) : this()
        {
            this._locator = locator;
            this.Factory = factory;

            _groups.Add(new SiteGroupViewModel(locator, this.Factory, "Favorites")); // Favorites
            _groups.Add(new SiteGroupViewModel(locator, this.Factory, "Delayed"));   // Delayed
            _groups.Add(new SiteGroupViewModel(locator, this.Factory, "Actives"));   // Actives
            _groups.Add(new SiteGroupViewModel(locator, this.Factory, "News"));      // News
            _groups.Add(new SiteGroupViewModel(locator, this.Factory, "Closed"));    // Closed

            // Load data asynchronously
            this.LoadDataAsync();
        }

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Gets all Days groupped
        /// </summary>
        public SiteGroupViewModelCollection Groups
        {
            get
            {
                return this._groups;
            }
        }

        /// <summary>
        /// Gets or sets the list of items found by Search Method.
        /// </summary>
        public ObservableCollection<SiteItemViewModel> SearchItemsFound { get; set; }

        #endregion

        #region METHODS

        /// <summary>
        /// Load data from the Factory.
        /// </summary>
        /// <returns></returns>
        public virtual async void LoadDataAsync()
        {
            this.IsWorking = true;

#if WINDOWS_PHONE_APP || WINDOWS_APP
            await this.RunAsync(async () =>
#else
            await System.Threading.Tasks.Task.Run(async () =>
#endif
            {
                Site[] allSites = await this.Factory.LoadAllSitesForCurrentUserAsync();

                // TODO: Remove these examples
                if (allSites.Length > 4)
                {
                    allSites[0].IsFavorite = true;
                    allSites[0].Status = Configuration.SiteStatusNew;
                    allSites[1].IsFavorite = true;
                    allSites[1].Status = Configuration.SiteStatusActive;
                    allSites[2].IsFavorite = true;
                    allSites[2].Status = Configuration.SiteStatusActive;
                    allSites[3].IsFavorite = true;
                    allSites[3].Status = Configuration.SiteStatusClosed;
                    allSites[4].IsFavorite = true;
                    allSites[4].Status = Configuration.SiteStatusActive;
                    allSites[4].PlannedDate = new DateTime(2014, 08, 02);
                }

                // List of groups
                SiteGroupViewModel groupFavorites = this.Groups[0]; 
                groupFavorites.Items.Clear();
                SiteGroupViewModel groupDelayed = this.Groups[1]; 
                groupDelayed.Items.Clear();
                SiteGroupViewModel groupActive = this.Groups[2]; 
                groupActive.Items.Clear();
                SiteGroupViewModel groupNew = this.Groups[3]; 
                groupNew.Items.Clear();
                SiteGroupViewModel groupClosed = this.Groups[4]; 
                groupClosed.Items.Clear();

                // DELAYED
                foreach (var item in allSites.Where(i => i.Status != Configuration.SiteStatusClosed && i.PlannedDate < DateTime.Now))
                {
                    await AddItemToGroup(groupFavorites, groupDelayed, item);
                }

                // ACTIVES
                foreach (var item in allSites.Where(i => i.Status == Configuration.SiteStatusActive && i.PlannedDate >= DateTime.Now))
                {
                    await AddItemToGroup(groupFavorites, groupActive, item);
                }

                // NEWS
                foreach (var item in allSites.Where(i => i.Status == Configuration.SiteStatusNew && i.PlannedDate >= DateTime.Now))
                {
                    await AddItemToGroup(groupFavorites, groupNew, item);
                }

                // CLOSED
                foreach (var item in allSites.Where(i => i.Status == Configuration.SiteStatusClosed))
                {
                    await AddItemToGroup(groupFavorites, groupClosed, item);
                }
            });


            this.IsWorking = false;
        }

        /// <summary>
        /// Returns the SiteViewItem associated to this SiteID (or null if not found).
        /// </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public async Task<SiteItemViewModel> GetSite(int siteID)
        {
            return this.GetAllSiteItems().FirstOrDefault(i => i.SiteID == siteID);
        }

        /// <summary>
        /// Search a keyword in Number or Address and updates SearchItemsFound property.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async Task Search(string keyword)
        {
            //this.IsWorking = true;

            this.SearchItemsFound.Clear();

            this.SearchItemsFound.AddRange(this.GetAllSiteItems().Where(i => i.Number.Contains(keyword) || i.Address.Contains(keyword)));

            //this.IsWorking = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<SiteItemViewModel> CreateNewSite()
        {
            SiteGroupViewModel groupFavorites = this.Groups[0];
            SiteGroupViewModel groupNew = this.Groups[3];

            // Create a new Site
            Site newSite = new Site();
            newSite.Status = Configuration.SiteStatusNew;
            newSite.EnterpriseName = this._locator.Authenticator.Company;

            // Add to the "News" Group
            return await this.AddItemToGroup(groupFavorites, groupNew, newSite);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="item"></param>
        private async Task<SiteItemViewModel> AddItemToGroup(SiteGroupViewModel favorite, SiteGroupViewModel group, Site item)
        {
            SiteItemViewModel newItem = new SiteItemViewModel(_locator, this.Factory, item);
            group.Items.Add(newItem);
            if (item.IsFavorite)
            {
                favorite.Items.Add(newItem);
            }
            return newItem;
        }

        /// <summary>
        /// Returns all sites loaded in memory.
        /// </summary>
        /// <returns></returns>
        private SiteItemViewModelCollection GetAllSiteItems()
        {
            SiteItemViewModelCollection allItems = new SiteItemViewModelCollection();
            allItems.AddRange(this.Groups[1].Items);
            allItems.AddRange(this.Groups[2].Items);
            allItems.AddRange(this.Groups[3].Items);
            allItems.AddRange(this.Groups[4].Items);
            return allItems;
        }

        #endregion
    }
}
