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
    public class SiteController : ControllerBase
    {

        protected void EditData(int siteId)
        {
            var rh = new RequestHelper<SiteModels>();
            var re = new RequestHelper<IEnumerable<EnterpriseModels>>();
            ViewBag.SiteID = siteId;
            if (Session["MessageSuccess"] != null)
            {
                ViewBag.MessageSuccess = Session["MessageSuccess"];
                Session["MessageSuccess"] = null;
            }
            ViewBag.model = rh.GetRequestToken(String.Format("Site/{0}", siteId), CurrentUser.Token);
            ViewBag.listEnterprices = re.GetRequestToken("Enterprises", CurrentUser.Token);
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (EnterpriseModels item in ViewBag.listEnterprices)
            {
                var listItem = new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.EnterpriseID.ToString()

                };
                if (siteId != 0 && ViewBag.model.EnterpriseID == long.Parse(item.EnterpriseID.ToString()))
                {
                    listItem.Selected = true;
                }
                listItems.Add(listItem);
                ViewBag.MessageSuccess = string.Empty;
            }
            ViewBag.ListEnterpriseSelectItems = listItems;

            var rphoto = new RequestHelper<IEnumerable<PhotosModels>>();
            ViewBag.PhotoList = rphoto.GetRequestToken(string.Format("Site/{0}/Photos", siteId), CurrentUser.Token).OrderBy(photo => photo.Timestamp).OrderBy(photo => photo.Tag).ToList();
            var rZip = new RequestHelper<string>();
            ViewBag.ZipURL = rZip.GetRequestToken(string.Format("Site/{0}/Photos/Zip", siteId), CurrentUser.Token);

            ViewBag.Longitude = ViewBag.model.Longitude;
            ViewBag.Latitude = ViewBag.model.Latitude;
        }
        // GET: Site
        [AuthorizeUserAttribute(AccessLevel = 2)]
        public ActionResult Index()
        {
            base.SetGlobalInformation();
            ViewBag.FontIcon = "fa-users";
            var rh = new RequestHelper<SiteModels[]>();

            ViewBag.ResultsSiteInformations = rh.GetRequestToken("Sites", CurrentUser.Token);


            return View();
        }

        // GET: Site
        [HttpGet]
        [AuthorizeUserAttribute(AccessLevel = 2)]
        public ActionResult Edit(int siteId = 0)
        {
            EditData(siteId);

            //SEND DATA TO SERVER

            base.SetGlobalInformation();
            ViewBag.FontIcon = "fa-pencil";

            return View("Edit", ViewBag.model);

        }

        // GET: Site
        [HttpPost]
        [AuthorizeUserAttribute(AccessLevel = 2)]
        public ActionResult Edit(int siteId, SiteModels model)
        {
            if (ModelState.IsValid)
            {

                ViewBag.FontIcon = "fa-pencil";
                var rh = new RequestHelper<SiteModels>();
                var re = new RequestHelper<IEnumerable<EnterpriseModels>>();
                ViewBag.SiteID = siteId;
                ViewBag.listEnterprices = re.GetRequestToken("Enterprises", CurrentUser.Token);
                List<SelectListItem> listItems = new List<SelectListItem>();
                foreach (EnterpriseModels item in ViewBag.listEnterprices)
                {
                    var listItem = new SelectListItem()
                    {
                        Text = item.Name,
                        Value = item.EnterpriseID.ToString()

                    };
                    if (model.EnterpriseID == long.Parse(item.EnterpriseID.ToString()))
                    {
                        listItem.Selected = true;
                    }
                    listItems.Add(listItem);

                }
                EditData(siteId);
                ViewBag.ListEnterpriseSelectItems = listItems;
                if (rh.PostRequestTokenBool("Site/", model, CurrentUser.Token))
                {
                    Session["MessageSuccess"] = "Your request is a success - Return to <a href=\"" + Url.Action("Index") + "\">Site liste</a>";
                    if (Session["MessageSuccess"] != null)
                    {
                        ViewBag.MessageSuccess = Session["MessageSuccess"];
                        Session["MessageSuccess"] = null;
                    }
                    return View();
                }
            }
            base.SetGlobalInformation();

            ViewBag.ErrorMessage = "An error occurs.";
            return View();
        }
    }
}