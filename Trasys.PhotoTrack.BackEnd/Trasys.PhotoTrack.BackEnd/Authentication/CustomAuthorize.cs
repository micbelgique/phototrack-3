using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trasys.PhotoTrack.BackEnd.Models;

namespace Trasys.PhotoTrack.BackEnd.Authentication
{

    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        // Custom property
        public int AccessLevel { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authroized = base.AuthorizeCore(httpContext);
            if (!authroized)
            {
                return false;
            }

            // Now check the session:
            var userDetails = httpContext.Session["UserDetails"] as ApplicationUser;
            if (userDetails == null)
            {
                // the session has expired
                return false;
            }
            if (userDetails.Type <= AccessLevel)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
