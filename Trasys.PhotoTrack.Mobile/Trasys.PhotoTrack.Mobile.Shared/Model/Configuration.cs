using System;
using System.Collections.Generic;
using System.Text;

namespace Trasys.PhotoTrack.Mobile.Model
{
    public static class Configuration
    {
        /// <summary>
        /// Gets the WebApi Base URL.
        /// </summary>
        public const string WebApiBaseUrl = "@URL_AZURE#"; // Set the correct URL to azure WebAPI

        public const string SiteStatusNew = "NEW";
        public const string SiteStatusClosed = "CLSD";
        public const string SiteStatusActive = "PGR";

        public const string PhotoStateBefore = "before";
        public const string PhotoStateDuring = "during";
        public const string PhotoStateAfter = "after";
        
    }
}
