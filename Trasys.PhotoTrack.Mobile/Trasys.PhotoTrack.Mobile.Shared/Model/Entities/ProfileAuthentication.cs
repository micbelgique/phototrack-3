using System;
using System.Collections.Generic;
using System.Text;

namespace Trasys.PhotoTrack.Mobile.Model.Entities
{
    public class ProfileAuthentication
    {
        public Profile ProfileLogged { get; set; }
        public string Token { get; set; }
        public AuthenticationResult AuthenticationResult { get; set; }
    }

    public enum AuthenticationResult
    {
        Success = 0,
        BadCredentials = 1,
        ProfileNotFound = 2,
        Unknown = 3
    }
}
