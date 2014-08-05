using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Trasys.Dev.Tools.Mvvm;
using Trasys.PhotoTrack.Mobile.Model;
using Trasys.PhotoTrack.Mobile.Model.Entities;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.UI;

namespace Trasys.PhotoTrack.Mobile.ViewModel
{
    /// <summary>
    /// Description of a Report
    /// </summary>
    public class SiteItemViewModel : ViewModelBase<Model.Factory>
    {
        private ViewModelLocator _locator = null;
        private ObservableCollection<PhotoViewModel> _photo = new ObservableCollection<PhotoViewModel>();
        private bool _photoLoaded = false;
        private string _address = string.Empty;
        private string _company = string.Empty;
        private string _number = string.Empty;
        private double _longitude = 0;
        private double _latitude = 0;
        
        /// <summary>
        /// Initializes a new empty instance, for Sample Data.
        /// </summary>
        public SiteItemViewModel()
        {
        }

        /// <summary>
        /// Initializes a new empty instance.
        /// </summary>
        /// <param name="factory"></param>
        public SiteItemViewModel(ViewModelLocator locator, Model.Factory factory)
        {
            this._locator = locator;
            this.Factory = factory;
        }

        /// <summary>
        /// Initializes a new instance filled with all properties of entitySite.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="entitySite"></param>
        internal SiteItemViewModel(ViewModelLocator locator, Model.Factory factory, Site entitySite) : this(locator, factory)
        {
            this.ImportFromModelSite(entitySite);
        }        

        /// <summary>
        /// Gets or sets the Site ID.
        /// </summary>
        public int SiteID { get; set; }

        /// <summary>
        /// Gets or sets the site number.
        /// </summary>
        public string Number
        {
            get
            {
                return _number;
            }
            set
            {
                SetProperty(ref _number, value);
            }
        }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                SetProperty(ref _address, value);
            }
        }

        /// <summary>
        /// Gets or sets the Site Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the company name.
        /// </summary>
        public string Company
        {
            get
            {
                return _company;
            }
            set
            {
                SetProperty(ref _company, value);
            }
        }

        /// <summary>
        /// Gets or sets the company ID.
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// Gets or sets the site longitude.
        /// </summary>
        public double Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                SetProperty(ref _longitude, value);
                OnPropertyChanged("Coordonates");
            }
        }

        /// <summary>
        /// Gets or sets the site latitude.
        /// </summary>
        public double Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                SetProperty(ref _latitude, value);
                OnPropertyChanged("Coordonates");
            }
        }

        /// <summary>
        /// Gets the full GPS coordonates.
        /// </summary>
        public string Coordonates
        {
            get
            {
                return String.Format("{0} - {1}", this.Longitude, this.Latitude);
            }
        }

        /// <summary>
        /// Gets or sets the Status of this site: NEW, CLSD (Closed), PRG (In progress).
        /// </summary>
        public string Status { get; set; }

        public string StatusFormatted
        {
            get
            {
                switch (Status)
                {
                    case Configuration.SiteStatusActive:
                        return "active";
                    case Configuration.SiteStatusClosed:
                        return "closed";
                    case Configuration.SiteStatusNew:
                        return "new";
                }
                return "";
            }
        }

        /// <summary>
        /// Gets or sets True if this site is flagged as Favorite by the user.
        /// </summary>
        public bool IsFavorite { get; set; }

        /// <summary>
        /// gets the color associated with the item
        /// </summary>
        public string RectangleFillColor
        {
            get
            {
                string color;

                switch (this.Status)
                {
                    case Configuration.SiteStatusNew:       // Blue
                        color = "#FF2699D5";
                        break;

                    case Configuration.SiteStatusClosed:    // Gray
                        color = "#FF404040";
                        break;

                    case Configuration.SiteStatusActive:    // Green
                        color = "#FF00B050";
                        break;

                    default:
                        color = "#FF2699D5";                // Blue
                        break;
                }

                if ((this.Status == Configuration.SiteStatusActive || this.Status == Configuration.SiteStatusNew) 
                    && this.PlannedDate < DateTime.Now)
                {
                    color = "#FFB73B18";                    // Red
                }

                // TODO: Pas si dans le groupe Favorites
                //if (this.IsFavorite)        
                //{
                //    color = "#FFFFC000";                    // Yellow
                //}

                return color;
            }
        }

        /// <summary>
        /// Gets the current location
        /// </summary>
        public List<Geopoint> Points
        {
            get
            {
                string[] splitted = Coordonates.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);

                BasicGeoposition myLocation = new BasicGeoposition
                {
                    Longitude = System.Convert.ToDouble(splitted[0]),
                    Latitude = System.Convert.ToDouble(splitted[1])
                };
                Geopoint point = new Geopoint(myLocation);
                return new List<Geopoint>(){point};
            }
        }

        /// <summary>
        /// Gets or sets the planification date of this job.
        /// </summary>
        public DateTime PlannedDate { get; set; }

        /// <summary>
        /// Gets or sets a list of photographies.
        /// </summary>
        public ObservableCollection<PhotoViewModel> Photos 
        {
            get
            {
                return _photo;
            }
        }

        /// <summary>
        /// Load all site details
        /// </summary>
        /// <returns></returns>
        public async Task LoadDetailsAsync()
        {
            if(!IsLoaded && !IsLoading)
                await LoadPhotosAsync();
        }

        private bool IsLoading { get; set; }
        private bool IsLoaded { get; set; }

        /// <summary>
        /// Refresh data from the Factory.
        /// </summary>
        /// <returns></returns>
        public virtual async void RefrechAsync(int siteID)
        {
            this.IsWorking = true;

        #if WINDOWS_PHONE_APP || WINDOWS_APP
            await this.RunAsync(async () =>
        #else
            await System.Threading.Tasks.Task.Run(async () =>
        #endif
            {
                Site site = new Site()
                {
                    SiteID = this.SiteID,
                    Number = this.Number,
                    Address = this.Address,
                    Description = this.Description,
                    EnterpriseName = this.Company,
                    EnterpriseID = this.CompanyID,
                    Status = "0",
                    Latitude = this.Latitude,
                    Longitude = this.Longitude
                };

                bool ok = await this.Factory.UpdateOneSiteAsync(site);
            });

            this.IsWorking = false;
        }

        /// <summary>
        /// Save data to the Factory.
        /// </summary>
        /// <returns></returns>
        public virtual async void UpdateAsync()
        {
            this.IsWorking = true;


            #if WINDOWS_PHONE_APP || WINDOWS_APP
            await this.RunAsync(async () =>
            #else
            await System.Threading.Tasks.Task.Run(async () =>
            #endif
            {
                Site site = new Site() 
                {
                    SiteID = this.SiteID,
                    Number = this.Number,
                    Address = this.Address,
                    Description = this.Description,
                    EnterpriseName = this.Company,
                    EnterpriseID = this.CompanyID,
                    Status = "0",
                    Latitude = this.Latitude,
                    Longitude = this.Longitude
                };

                bool ok = await this.Factory.UpdateOneSiteAsync(site);
            });

            this.IsWorking = false;
        }

        /// <summary>
        /// Add a new Photo to this Site.
        /// </summary>
        /// <param name="picture"></param>
        public async Task AddNewPhotoAsync(StorageFile picture)
        {
            string defaultTag = Configuration.PhotoStateBefore;

            PhotoViewModel newPhoto = new PhotoViewModel(this.Factory) { SiteID = this.SiteID, Tag = defaultTag, PhotoID = Guid.NewGuid().ToString(), Timestamp = DateTime.Now };
            this.Photos.Add(newPhoto);

            newPhoto.Url = await this.Factory.SavePhotoAsync(picture, this.SiteID, defaultTag);
        }

        /// <summary>
        /// Load all photos
        /// </summary>
        /// <returns></returns>
        private async Task LoadPhotosAsync()
        {
            IsLoading = true;
            var photos = await this.Factory.LoadPhotoForSite(this.SiteID);

            foreach (Photo item in photos)
            {
                _photo.Add(new PhotoViewModel(this.Factory) { SiteID = this.SiteID, Url = item.Url, Tag = Configuration.PhotoStateBefore, PhotoID = item.ID, Timestamp = item.Timestamp });
            }
            IsLoaded = true;
            IsLoading = false;
        }

        /// <summary>
        /// Fill all curent object properties with Entities Site item.
        /// </summary>
        /// <param name="item"></param>
        internal void ImportFromModelSite(Trasys.PhotoTrack.Mobile.Model.Entities.Site item)
        {
            this.Address = item.Address;
            this.Company = String.IsNullOrEmpty(item.EnterpriseName) ? String.Empty : item.EnterpriseName;
            this.CompanyID = item.EnterpriseID;
            this.Description = item.Description;
            this.SiteID = item.SiteID;
            this.IsFavorite = false;
            this.Latitude = item.Latitude;
            this.Longitude = item.Longitude;
            this.Number = item.Number;
            this.Status = item.Status;
            this.PlannedDate = item.PlannedDate;
        }
    }

    /// <summary>
    /// List of items.
    /// </summary>
    public class SiteItemViewModelCollection : ObservableCollection<SiteItemViewModel>
    {

    }

}
