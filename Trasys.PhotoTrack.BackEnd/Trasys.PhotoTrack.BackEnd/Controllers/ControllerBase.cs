using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Trasys.PhotoTrack.BackEnd.Models;

namespace Trasys.PhotoTrack.BackEnd.Controllers
{
    public class ControllerBase : Controller
    {
        protected void SetGlobalInformation()
        {

            ViewBag.UserName = CurrentUser.FullName;
            ViewBag.UserType = CurrentUser.TypeDescription;
            ViewBag.UserEmail = CurrentUser.Email;
            ViewBag.FontIcon = "fa-list";
            string hash = string.Empty;
            using (MD5 md5Hash = MD5.Create())
            {
                hash = Trasys.PhotoTrack.BackEnd.Tools.HtmlHelperExtensions.GetMd5Hash(md5Hash, ViewBag.UserEmail);
            }

            ViewBag.UserEmailHashed = hash;

        }

        protected ApplicationUser CurrentUser
        {
            get
            {
                return Session["UserDetails"] as ApplicationUser;
            }
        }
    }
}