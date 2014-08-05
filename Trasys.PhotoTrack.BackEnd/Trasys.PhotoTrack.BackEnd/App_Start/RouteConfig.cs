using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Trasys.PhotoTrack.BackEnd
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "LoginPage",
                url: "Login",
                defaults: new { controller = "Account", action = "Login", returnUrl = "/" }
            );

            routes.MapRoute(
                name: "AccountsList",
                url: "Accounts",
                defaults: new { controller = "Account", action = "List" }
            );

            routes.MapRoute(
                name: "EditSite",
                url: "Site/Edit/{siteId}",
                defaults: new { controller = "Site", action = "Edit", siteID = 0 }
            );

            routes.MapRoute(
                name: "CreateSite",
                url: "Site/New/",
                defaults: new { controller = "Site", action = "Edit", siteID = 0 }
            );

            routes.MapRoute(
                name: "EditAccount",
                url: "Account/Edit/{userId}",
                defaults: new { controller = "Account", action = "Edit", userId = 0 }
            );


            routes.MapRoute(
                name: "EnterprisesRoute",
                url: "Enterprise/{enterpriseID}",
                defaults: new { controller = "Enterprise", action = "Index", enterpriseID = -1 }
            );

            routes.MapRoute(
               name: "Enterprises",
               url: "Enterprise",
               defaults: new { controller = "Enterprise", action = "Index", enterpriseID = -1 }
           );

            routes.MapRoute(
                name: "LogOff",
                url: "LogOff",
                defaults: new { controller = "Account", action = "LogOff" }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
               
            );

        }
    }
}
