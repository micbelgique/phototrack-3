using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trasys.PhotoTrack.BackEnd.Authentication;
using Trasys.PhotoTrack.BackEnd.Models;
using Trasys.PhotoTrack.BackEnd.Tools;

namespace Trasys.PhotoTrack.BackEnd.Controllers
{
    public class EnterpriseController : ControllerBase
    {
        // GET: Company
        [AuthorizeUserAttribute(AccessLevel = 2)]
        public ActionResult Index(int enterpriseID = -1)
        {
            base.SetGlobalInformation();
            ViewBag.FontIcon = "fa-building-o";
            var rh = new RequestHelper<EnterpriseModels[]>();

            var list = rh.GetRequestToken("Enterprises", CurrentUser.Token);
            ViewBag.ResultsEnterpriseInformations = list;
            ViewBag.IsEdit = enterpriseID != -1;

            if (enterpriseID >= 0)
            {
                var model = list.Where(it => it.EnterpriseID == enterpriseID).FirstOrDefault();
                if (model != null)
                {
                    ViewBag.CompanyName = model.Name;
                }

                return View("Index", model);
            }


            return View();
        }
    }
}