using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trasys.PhotoTrack.Api.Models
{
    public class ProfileAuthentication
    {
        public Profile ProfileLogged { get; set; }
        public Guid Token { get; set; }
        public AuthenticationResult AuthenticationResult { get; set; }
    }
}