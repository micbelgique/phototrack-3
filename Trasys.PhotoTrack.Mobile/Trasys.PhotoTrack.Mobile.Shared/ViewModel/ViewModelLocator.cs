using System;
using System.Collections.Generic;
using System.Text;

namespace Trasys.PhotoTrack.Mobile.ViewModel
{
    /// <summary>
    /// Engine factory to all View Models
    /// </summary>
    public class ViewModelLocator : Trasys.Dev.Tools.Mvvm.ViewModelLocatorBase<Model.Factory>
    {
        private SiteViewModel _sitesViewModel = null;
        private AuthenticateViewModel _authenticator = null;
        private CompanyViewModel _companyViewModel = null;

        /// <summary>
        /// Initializes a new instance of ViewModelLocator, based on a Model Factory to retrieve data.
        /// </summary>
        /// <param name="factory"></param>
        public ViewModelLocator(Model.Factory factory)
        {
            this.Factory = factory;
        }

        /// <summary>
        /// Gets a reference to the authentication module
        /// </summary>
        public AuthenticateViewModel Authenticator
        {
            get
            {
                if(_authenticator==null)
                {
                    _authenticator = new AuthenticateViewModel(this, this.Factory);
                }
                return _authenticator;
            }
        }

        /// <summary>
        /// Gets a reference to the Sites ViewModel.
        /// </summary>
        public SiteViewModel Sites
        {
            get
            {
                if (_sitesViewModel == null)
                {
                    _sitesViewModel = new SiteViewModel(this, this.Factory);
                }
                return _sitesViewModel;
            }
            private set
            {
                this._sitesViewModel = value;
            }
        }

        /// <summary>
        /// Gets a reference to all companies ViewModel.
        /// </summary>
        //public CompanyViewModel Companies
        //{
        //    get
        //    {
        //        if (_companyViewModel == null)
        //        {
        //            _companyViewModel = new CompanyViewModel(this, this.Factory);
        //        }
        //        return _companyViewModel;
        //    }
        //    private set
        //    {
        //        this._companyViewModel = value;
        //    }
        //}
    }
}
