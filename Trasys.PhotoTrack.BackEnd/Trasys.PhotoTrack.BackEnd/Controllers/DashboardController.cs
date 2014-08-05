using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Trasys.PhotoTrack.BackEnd.Authentication;
using Trasys.PhotoTrack.BackEnd.Models;
using Trasys.PhotoTrack.BackEnd.Tools;

namespace Trasys.PhotoTrack.BackEnd.Controllers
{
    public class DashboardController : ControllerBase
    {
        [AuthorizeUserAttribute(AccessLevel = 2)]
        public ActionResult Index()
        {
            SetGlobalInformation();

            var re = new RequestHelper<DashboardModel>();

            ViewBag.DashboardData = re.GetRequestToken("Dashboard", CurrentUser.Token);
           
            return View();
        }

    }
}