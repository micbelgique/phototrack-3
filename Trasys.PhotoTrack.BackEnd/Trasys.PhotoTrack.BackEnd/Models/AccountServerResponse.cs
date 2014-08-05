using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trasys.PhotoTrack.BackEnd.Models
{
    public class AccountServerResponse
    {
        public ApplicationUser ProfileLogged { get; set; }
        public string Token { get; set; }
        public int AuthenticationResult { get; set; }
    }
}