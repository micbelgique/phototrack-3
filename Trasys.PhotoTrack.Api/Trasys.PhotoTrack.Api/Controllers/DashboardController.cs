using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Trasys.PhotoTrack.Api.Models;

namespace Trasys.PhotoTrack.Api.Controllers
{
    public class DashboardController : BaseController
    {
        /// <summary>
        /// Search sites
        /// </summary>
        /// <param name="token"></param>
        /// <param name="search">Search string</param>
        /// <returns></returns>
        [Route("api/{token}/Dashboard/")]
        [HttpGet]
        public DashboardSummary Get(string token)
        {
            if (CheckToken(token) == null)
                return null;
            return this.GetFactory().GetDashboardSummary();
        }
    }
}
