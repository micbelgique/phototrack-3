using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Trasys.PhotoTrack.Mobile.Dev.Apps.Tools.Web;
using Windows.Storage;

namespace Trasys.PhotoTrack.Mobile.Model
{
    public class Factory
    {
        /// <summary>
        /// Initializes a new instance of Factory
        /// </summary>
        public Factory()
        {
            this.Api = new WebApi(Configuration.WebApiBaseUrl);
            this.CurrentUser = new User(this);
        }

        /// <summary>
        /// Gets a reference to the current WebAPI Helper Manager.
        /// </summary>
        public WebApi Api { get; private set; }

        /// <summary>
        /// Gets a reference to the current authenticate User.
        /// </summary>
        public User CurrentUser { get; private set; }

        public bool IsNetWorkAvailable
        {
            get
            {
                return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            }
        }

        /// <summary>
        /// Updates the current site to server
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public async Task<bool> UpdateOneSiteAsync(Entities.Site site)
        {
            return await this.Api.Post<bool, Entities.Site>("api/{0}/Site", site, this.CurrentUser.Token);
        }

        /// <summary>
        /// Load the current site from server
        /// </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public async Task<Entities.SiteDetails> LoadOneSiteAsync(int siteID)
        {
            return await this.Api.Get<Entities.SiteDetails>("api/{0}/Site/{1}", this.CurrentUser.Token, siteID);
        }

        /// <summary>
        /// Load all sites for the current logged user.
        /// </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public async Task<Entities.Site[]> LoadAllSitesForCurrentUserAsync()
        {
            //return await this.Api.Get<Entities.Site[]>("api/{0}/Profile/Sites", this.CurrentUser.Token);
            return await this.Api.Get<Entities.Site[]>("api/{0}/Sites", this.CurrentUser.Token);
        }

        /// <summary>
        /// Load all photo for specified site.
        /// </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public async Task<Entities.Photo[]> LoadPhotoForSite(int siteID)
        {
            return await this.Api.Get<Entities.Photo[]>("api/{0}/Site/{1}/Photos", this.CurrentUser.Token, siteID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="siteID"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task<string> SavePhotoAsync(StorageFile picture, int siteID, string tag)
        {
            FormDataPostHelper formDataPost = new FormDataPostHelper();
            string fileName = picture.Name;
            string extension = picture.FileType;
            DateTimeOffset created = picture.DateCreated;

            byte[] data = await formDataPost.ReadFile(picture);

            // Generate post objects
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("filename", fileName);
            postParameters.Add("fileformat", extension);
            postParameters.Add("file", new FileParameter(data, fileName, "application/image"));

            // Create request and receive response
            string postURL = Configuration.WebApiBaseUrl + String.Format("api/{0}/Site/{1}/UploadPhoto/{2}/{3}", this.CurrentUser.Token, siteID, tag, created.ToString("yyyy-MM-dd"));
            HttpWebResponse webResponse = new FormDataPostHelper().MultipartFormDataPost(postURL, "PhotoTrack", postParameters);

            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string fullResponse = responseReader.ReadToEnd();
            webResponse.Dispose();

            return this.Api.JsonDeserialize<string>(fullResponse);
        }

        /// <summary>
        /// Load all companies available.
        /// </summary>
        /// <returns></returns>
        public async Task<Entities.Enterprise[]> LoadEnterprises()
        {
            return await this.Api.Get<Entities.Enterprise[]>("api/{0}/Enterprises", this.CurrentUser.Token);
        }
    }
}
