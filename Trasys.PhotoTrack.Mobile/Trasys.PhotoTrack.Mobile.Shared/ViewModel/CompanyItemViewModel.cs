using System;
using System.Collections.Generic;
using System.Text;
using Trasys.Dev.Tools.Mvvm;

namespace Trasys.PhotoTrack.Mobile.ViewModel
{
    public class CompanyItemViewModel : ViewModelBase<Model.Factory>
    {
        private ViewModelLocator _locator = null;

        /// <summary>
        /// Initializes a new empty instance, for Sample Data.
        /// </summary>
        public CompanyItemViewModel()
        {
        }

        /// <summary>
        /// Initializes a new empty instance.
        /// </summary>
        /// <param name="factory"></param>
        public CompanyItemViewModel(ViewModelLocator locator, Model.Factory factory)
        {
            this._locator = locator;
            this.Factory = factory;
        }

        /// <summary>
        /// Gets or sets the ID of this company.
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// Gets or sets the name of this company.
        /// </summary>
        public string Name { get; set; }
    }
}
