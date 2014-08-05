using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Trasys.PhotoTrack.Api.Models;

namespace Trasys.PhotoTrack.Api.Controllers
{
    public class ProfileController : BaseController
    {
        /// <summary>
        /// Login the user based on the supplied credentials
        /// 0 = Success
        /// 1 = BadCredentials
        /// 2 = ProfileNotFound
        /// 3 = Unknown
        /// </summary>
        /// <param name="credentials">Login / Password</param>
        /// <returns>Returns the created token</returns>
        [Route("api/Profile/Login")]
        [HttpPost]
        public ProfileAuthentication Login(Credentials credentials)
        {
            return this.GetFactory().Login(credentials.Login, credentials.Password);
        }

        /// <summary>
        /// Get all profiles
        /// </summary>
        /// <param name="token">Security token</param>
        /// <returns></returns>
        [Route("api/{token}/Profiles")]
        [HttpGet]
        public Profile[] Search(string token)
        {
            if (CheckToken(token) == null)
                return null;
            return this.GetProfileFactory().GetProfiles();
        }

        /// <summary>
        /// Get all profiles
        /// </summary>
        /// <param name="token">Security token</param>
        /// <returns></returns>
        [Route("api/{token}/Profile/{profileID}")]
        [HttpGet]
        public Profile Get(string token,long profileID)
        {
            if (CheckToken(token) == null)
                return null;
            return this.GetProfileFactory().GetProfile(profileID);
        }

        /// <summary>
        /// Search profiles (FirstName, lastName, email)
        /// </summary>
        /// <param name="token">Security token</param>
        /// <param name="search">Search string</param>
        /// <returns></returns>
        [Route("api/{token}/Profile/search/{search}")]
        [HttpGet]
        public Profile[] Search(string token, string search)
        {
            if (CheckToken(token) == null)
                return null;
            return this.GetProfileFactory().GetProfiles(search);
        }

        /// <summary>
        /// Get sites of a profile (determined by the token)
        /// </summary>
        /// <param name="token">Security token</param>
        /// <returns></returns>
        [Route("api/{token}/Profile/Sites/")]
        [HttpGet]
        public Site[] GetSites(string token)
        {
            long? profileID = CheckToken(token);
            if (profileID == null)
                return null;
            return this.GetSiteFactory().GetSites(profileID.Value);
        }

        /// <summary>
        /// Creates a profile
        /// </summary>
        /// <param name="token">Security token</param>
        /// <param name="profile">Profile to create. ID supplied will be ignored</param>
        public bool Post(string token, Profile profile)
        {
            if (CheckToken(token) == null)
                return false;
            return this.GetProfileFactory().CreateProfile(profile);
        }

        /// <summary>
        /// Delete a profile
        /// </summary>
        /// <param name="token">Security token</param>
        /// <param name="id">Id of the profile to delete</param>
        public void Delete(string token, int id)
        {
            if (CheckToken(token) == null)
                return;
            this.GetProfileFactory().DeleteProfile(id);
        }
    }
}
