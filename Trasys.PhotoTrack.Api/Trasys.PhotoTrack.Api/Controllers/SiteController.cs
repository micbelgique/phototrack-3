using Ionic.Zip;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Trasys.PhotoTrack.Api.Models;

namespace Trasys.PhotoTrack.Api.Controllers
{
    public class SiteController : BaseController
    {
        private static List<String> messages = new List<string>();
        /// <summary>
        /// Get the details of a specific site
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public SiteDetails Get(string token, int id)
        {
            if (CheckToken(token) == null)
                return null;
            return this.GetSiteFactory().GetSite(id);
        }

        /// <summary>
        /// Login the user based on the supplied credentials
        /// 0 = Success
        /// 1 = BadCredentials
        /// 2 = ProfileNotFound
        /// 3 = Unknown
        /// </summary>
        /// <param name="credentials">Login / Password</param>
        /// <returns>Returns the created token</returns>
        [Route("api/{token}/Site/{siteID}/Assign/{id}")]
        [HttpGet]
        public bool AssignSite(string token, long siteID, long id)
        {
            if (CheckToken(token) == null)
                return false;
            return this.GetSiteFactory().AssignSite(id, siteID);
        }

        /// <summary>
        /// Create a new site
        /// </summary>
        /// <param name="token"></param>
        /// <param name="site"></param>
        public bool Post(string token, Site site)
        {
            messages.Add(TraceCall());
            if (CheckToken(token) == null)
                return false;
            return this.GetSiteFactory().CreateSite(site);
        }

        /// <summary>
        /// Get all sites
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("api/{token}/Sites")]
        [HttpGet]
        public Site[] GetAllSites(string token)
        {
            if (CheckToken(token) == null)
                return null;
            return this.GetSiteFactory().GetSites();
        }

        /// <summary>
        /// Get messages
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("api/Messages")]
        [HttpGet]
        public string[] GetAllMessages()
        {
            return messages.ToArray();
        }


        /// <summary>
        /// Search sites
        /// </summary>
        /// <param name="token"></param>
        /// <param name="search">Search string</param>
        /// <returns></returns>
        [Route("api/{token}/Site/Search/{search}")]
        [HttpGet]
        public Site[] Search(string token, string search)
        {
            if (CheckToken(token) == null)
                return null;
            return this.GetSiteFactory().GetSites(search);
        }

        /// <summary>
        /// Upload photo for a specific site
        /// </summary>
        /// <returns></returns>
        [Route("api/{token}/Site/{siteID}/UploadPhoto/{tag}/{TimeStamp}")]
        [HttpPost]
        public async Task<string> UploadPhoto(string token, long siteID, string tag, DateTime TimeStamp)
        {
            if (CheckToken(token) == null)
                return null;

            string ServerUploadFolder = Path.GetTempPath();
            // Verify that this is an HTML Form file upload request
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
            }

            // Create a stream provider for setting up output streams
            MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(ServerUploadFolder);

            Guid fileID = Guid.NewGuid();
            // Read the MIME multipart asynchronously content using the stream provider we just created.
            await Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith(x =>
                {
                    CloudBlobContainer blobContainer = BlobHelper.GetWebApiContainer();
                    var blob = blobContainer.GetBlockBlobReference(fileID.ToString() + ".jpg");
                    using (var fileStream = System.IO.File.OpenRead(streamProvider.FileData[0].LocalFileName))
                    {
                        blob.UploadFromStream(fileStream);
                    }

                    // Create the record in SQL
                    DateTime newTimeStamp = new DateTime(TimeStamp.Year, TimeStamp.Month, TimeStamp.Day, TimeStamp.Hour, TimeStamp.Minute, TimeStamp.Second);
                    this.GetSiteFactory().AddPhoto(siteID, fileID, new string[] { tag }, newTimeStamp);
                });
            // Create response
            return fileID.ToString() + ".jpg";
        }


        /// <summary>
        /// Get photos for a specific site
        /// </summary>
        /// <returns></returns>
        [Route("api/{token}/Site/{siteID}/Photos")]
        [HttpGet]
        public Photo[] GetPhotos(string token, long siteID)
        {
            if (CheckToken(token) == null)
                return null;

            List<string> files = new List<string>();

            Photo[] foundPhotos = this.GetSiteFactory().GetPhotos(siteID);
            return foundPhotos;
        }

        /// <summary>
        /// Update tag of a photo
        /// </summary>
        /// <returns></returns>
        [Route("api/{token}/Site/{siteID}/Photo/{photoID}/{newTag}")]
        [HttpGet]
        public bool GetPhotos(string token, long siteID,string photoID,string newTag)
        {
            if (CheckToken(token) == null)
                return false;

            return this.GetSiteFactory().UpdateTag(siteID, photoID, newTag);
        }

        /// <summary>
        /// Get photos for a specific site
        /// </summary>
        /// <returns></returns>
        [Route("api/{token}/Site/{siteID}/Photos/Zip")]
        [HttpGet]
        public string GetZippedPhotos(string token, long siteID)
        {
            if (CheckToken(token) == null)
                return null;

            Photo[] foundPhotos = this.GetSiteFactory().GetPhotos(siteID);

            //Get photos from urls
            // Read the MIME multipart asynchronously content using the stream provider we just created.
            CloudBlobContainer blobContainer = BlobHelper.GetWebApiContainer();

            Dictionary<string, MemoryStream> photos = new Dictionary<string, MemoryStream>();
            string fileName = String.Format("{1}-Site{0}.zip", siteID, DateTime.Now.ToString("yymmdd"));

            foreach (Photo photo in foundPhotos)
            {
                var blob = blobContainer.GetBlockBlobReference(photo.ID + ".jpg");
                byte[] downloadedPhoto = new byte[] { };
                MemoryStream test = new MemoryStream();
                blob.DownloadToStream(test);
                photos.Add(photo.ID, test);
                test.Seek(0,SeekOrigin.Begin); 
            }
           

            ZipFile zip = new ZipFile(fileName);

            foreach (string key in photos.Keys)
            {
                zip.AddEntry(key + ".jpg", photos[key]);
            }

            MemoryStream toUpload = new MemoryStream();
            zip.Save(toUpload);
            var blobUpload = blobContainer.GetBlockBlobReference(fileName);
            toUpload.Seek(0, SeekOrigin.Begin); 
            blobUpload.UploadFromStream(toUpload);
            return baseUrl + fileName;
        }

    }
}
