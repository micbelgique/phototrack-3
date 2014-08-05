using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Trasys.PhotoTrack.Mobile.Model.Entities;

namespace Trasys.PhotoTrack.Mobile.Model
{
    /// <summary>
    /// Description of a logged user
    /// </summary>
    public class User : Profile
    {
        private Factory _factory = null;

        public User(Factory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Gets or sets the Token to authenticate this current user to all WebAPI.
        /// </summary>
        public string Token { get; set; }
        
        /// <summary>
        /// Athenticate the user and fill all properties.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> Authenticate(string login, string password)
        {
            ProfileAuthentication result = await _factory.Api.Post<ProfileAuthentication, Credentials>("api/Profile/Login", new Credentials() { Login = login, Password = password });

            if (result != null && result.AuthenticationResult == AuthenticationResult.Success)
            {
                this.Email = result.ProfileLogged.Email;
                this.FirstName = result.ProfileLogged.FirstName;
                this.LastName = result.ProfileLogged.LastName;
                this.Login = result.ProfileLogged.Login;
                this.Password = result.ProfileLogged.Password;
                this.ProfileID = result.ProfileLogged.ProfileID;
                this.Type = result.ProfileLogged.Type;
                this.EnterpriseID = result.ProfileLogged.EnterpriseID;
                this.EnterpriseName = result.ProfileLogged.EnterpriseName;
                this.Token = result.Token;
                return true;
            }
            else
            {
                this.Email = String.Empty;
                this.FirstName = String.Empty;
                this.LastName = String.Empty;
                this.Login = String.Empty;
                this.Password = String.Empty;
                this.ProfileID = 0;
                this.Type = 0;
                this.Token = String.Empty;
                this.EnterpriseID = 0;
                this.EnterpriseName = String.Empty;
                return false;
            }

        }
    }
}
