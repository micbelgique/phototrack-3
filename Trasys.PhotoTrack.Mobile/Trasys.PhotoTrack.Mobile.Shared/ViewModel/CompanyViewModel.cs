using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Trasys.Dev.Tools.Mvvm;

namespace Trasys.PhotoTrack.Mobile.ViewModel
{
    public class CompanyViewModel : ViewModelBase<Model.Factory>
    {
        private ViewModelLocator _locator = null;
        
        /// <summary>
        /// Initializes a new empty instance, for Sample Data.
        /// </summary>
        public CompanyViewModel()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of this ViewModel.
        /// </summary>
        public CompanyViewModel(ViewModelLocator locator, Model.Factory factory)
        {
            this._locator = locator;
            this.Factory = factory;
            
            // Load data asynchronously
            this.LoadDataAsync();
        }

        /// <summary>
        /// Gets or sets a list of Company.
        /// </summary>
        public ObservableCollection<CompanyItemViewModel> Items { get; set; }

        /// <summary>
        /// Load data from the Factory.
        /// </summary>
        /// <returns></returns>
        public virtual async void LoadDataAsync()
        {
            this.IsWorking = true;

            Model.Entities.Enterprise[] companies = await this.Factory.LoadEnterprises();

            this.Items.Clear();
            foreach (Model.Entities.Enterprise company in companies)
            {
                this.Items.Add(new CompanyItemViewModel(_locator, this.Factory) { CompanyID = company.EntepriseID, Name = company.Name });
            }

            this.IsWorking = false;
        }

    }
}
