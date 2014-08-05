using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Trasys.Dev.Tools.Mvvm;

namespace Trasys.PhotoTrack.Mobile.ViewModel
{
    public class AuthenticateViewModel : ViewModelBase<Model.Factory>
    {
        private ViewModelLocator _locator = null;

        /// <summary>
        /// Create a new instance of the authentication module
        /// </summary>
        /// <param name="factory"></param>
        public AuthenticateViewModel(ViewModelLocator locator, Model.Factory factory):base(factory)
        {
            this._locator = locator;
        }

        /// <summary>
        /// Gets or sets the current Login
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the current password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the user company.
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Authenticate the user and return True if OK.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Authenticate()
        {
            return await this.Factory.CurrentUser.Authenticate(this.Login, this.Password);
        }

        /// <summary>
        /// Authenticate the user and return True if OK.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> Authenticate(string login, string password)
        {
            this.Login = login;
            this.Password = password;
            
            var result = await this.Factory.CurrentUser.Authenticate(this.Login, this.Password);
            this.Company = this.Factory.CurrentUser.EnterpriseName;
            return result;
        }
    }
}
