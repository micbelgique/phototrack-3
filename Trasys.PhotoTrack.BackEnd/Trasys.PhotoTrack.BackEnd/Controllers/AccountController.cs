using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using Trasys.PhotoTrack.BackEnd.Models;
using Trasys.PhotoTrack.BackEnd.Tools;
using System.Security.Principal;
using System.Web.Security;
using Trasys.PhotoTrack.BackEnd.Authentication;

namespace Trasys.PhotoTrack.BackEnd.Controllers
{
    [Authorize]
    public class AccountController : ControllerBase
    {

        protected void EditData(int userId)
        {
            ViewBag.FontIcon = "fa-user";
            var rh = new RequestHelper<ApplicationUser>();
            var re = new RequestHelper<IEnumerable<EnterpriseModels>>();
            ViewBag.UserId = userId;
            ViewBag.listEnterprices = re.GetRequestToken("Enterprises", CurrentUser.Token);
            ViewBag.model = rh.GetRequestToken(String.Format("Profile/{0}", userId), CurrentUser.Token);
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (EnterpriseModels item in ViewBag.listEnterprices)
            {
                var listItem = new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.EnterpriseID.ToString()

                };
                listItems.Add(listItem);
            }



            ViewBag.ListEnterpriseSelectItems = listItems;
        }
        public AccountController()
        {
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var request = new RequestHelper<LoginUser, AccountServerResponse>();
                var user = request.PostRequest("Profile/Login", new LoginUser() { Login = model.Email, Password = model.Password });


                if (user != null)
                {
                    user.ProfileLogged.Token = user.Token;
                    Session["User"] = user;
                    Session["UserDetails"] = user.ProfileLogged;
                    FormsAuthentication.SetAuthCookie(user.ProfileLogged.Login, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpGet]
        public ActionResult LogOff()
        {
            Session["User"] = null;
            Session["UserDetail"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [AuthorizeUserAttribute(AccessLevel = 2)]
        public ActionResult List()
        {
            SetGlobalInformation();

            ViewBag.FontIcon = "fa-list";
            var rh = new RequestHelper<ApplicationUser[]>();

            ViewBag.ResultsUsersInformations = rh.GetRequestToken("Profiles", CurrentUser.Token);

            return View();
        }


        [HttpGet]
        [AuthorizeUserAttribute(AccessLevel = 2)]
        public ActionResult Edit(int userId = 0)
        {
            EditData(userId);

            //SEND DATA TO SERVER

            base.SetGlobalInformation();
            ViewBag.FontIcon = "fa-pencil";

            return View("Edit", ViewBag.model);

        }

        // GET: Site
        [HttpPost]
        [AuthorizeUserAttribute(AccessLevel = 2)]
        public ActionResult Edit(int userId, ApplicationUser model)
        {
            if (ModelState.IsValid)
            {

                var rh = new RequestHelper<ApplicationUser>();
                EditData(userId);
                if (rh.PostRequestTokenBool("Account/", model, CurrentUser.Token))
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