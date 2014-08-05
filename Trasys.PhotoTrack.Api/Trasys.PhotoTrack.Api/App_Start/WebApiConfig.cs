using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Trasys.PhotoTrack.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "LoginApi",
                routeTemplate: "api/Login/",
                defaults: new { controller = "Profile", action = "Login", segment = "Segment" }
           );


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{token}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
