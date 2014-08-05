using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using Trasys.PhotoTrack.Api.Models;

namespace Trasys.PhotoTrack.Api.Controllers
{
    public class BaseController : ApiController
    {
        protected string baseUrl = "";
        protected Factory _factory = null;
 
        public BaseController()
        {
            _factory = new Factory();
        }

        protected Factory GetFactory()
        {
            if(_factory == null)
                _factory = new Factory();
            return _factory;
        }

        protected ProfileFactory GetProfileFactory()
        {
            return new ProfileFactory(this.GetFactory());
        }

        protected SiteFactory GetSiteFactory()
        {
            return new SiteFactory(this.GetFactory());
        }

        protected EnterpriseFactory GetEnterpriseFactory()
        {
            return new EnterpriseFactory(this.GetFactory());
        }

        protected long? CheckToken(string token)
        {
            Guid suppliedToken = Guid.Empty;
            if (!Guid.TryParse(token, out suppliedToken))
            {
                return null;
            }
            return this.GetFactory().CheckToken(suppliedToken);
        }

        // <summary>
        /// Trace a WebApi Call
        ///     Extract the url and the content of the request and sent it to the logger with trace level
        /// </summary>
        public string TraceCall()
        {
            StringBuilder content = new StringBuilder();

            content.AppendFormat("Call url : {0} - Thread : {1}", HttpContext.Current.Request.Url, Thread.CurrentThread.ManagedThreadId);

            if (HttpContext.Current.Request.RequestType == "POST")
            {
                content.AppendFormat("\t -  content : {0}", GetRequestRawContent());
            }
            return content.ToString();
        }

        /// <summary>
        /// Returns the current request raw content
        /// </summary>
        /// <returns></returns>
        protected string GetRequestRawContent()
        {
            string content = string.Empty;
            try
            {
                if (HttpContext.Current.Request.InputStream.CanSeek)
                {
                    HttpContext.Current.Request.InputStream.Seek(0, System.IO.SeekOrigin.Begin);
                }

                using (System.IO.StreamReader reader = new System.IO.StreamReader(HttpContext.Current.Request.InputStream))
                {
                    content = reader.ReadToEnd();
                }
            }
            catch
            {
            }
            return content;
        }
    }
}