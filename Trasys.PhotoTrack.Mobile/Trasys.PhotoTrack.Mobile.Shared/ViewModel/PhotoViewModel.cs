using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Trasys.Dev.Tools.Mvvm;
using Trasys.PhotoTrack.Mobile.Model;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Trasys.PhotoTrack.Mobile.ViewModel
{
    public class PhotoViewModel : ViewModelBase<Model.Factory>
    {
        private string _tag = String.Empty;
        private string _url = String.Empty;

        /// <summary>
        /// Initializes a new instance of PhotoViewModel
        /// </summary>
        /// <param name="factory"></param>
        public PhotoViewModel(Factory factory)
        {
            this.Factory = factory;
        }

        /// <summary>
        /// Gets or sets a reference to a Site
        /// </summary>
        public int SiteID { get; set; }

        /// <summary>
        /// Gets or sets the ID of picture.
        /// </summary>
        public string PhotoID { get; set; }

        /// <summary>
        /// Gets or sets the URL photo filename
        /// </summary>
        public string Url
        {
            get { return _url; }
            set
            {
                this.SetProperty(ref _url, value);
            }
        }

        /// <summary>
        /// Gets or sets the Tag associated to this photo.
        /// </summary>
        public string Tag
        {
            get { return _tag; }
            set 
            { 
                this.SetProperty(ref _tag, value);
            }
        }
        
        /// <summary>
        /// Gets or sets the date when picture was taken.
        /// </summary>
        public DateTime Timestamp { get; set; }

    }
}
