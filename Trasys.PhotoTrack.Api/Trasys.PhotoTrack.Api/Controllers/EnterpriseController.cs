using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Trasys.PhotoTrack.Api.Models;

namespace Trasys.PhotoTrack.Api.Controllers
{
    public class EnterpriseController : BaseController
    {
        /// <summary>
        /// Get all companies
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("api/{token}/Enterprises")]
        [HttpGet]
        public Enterprise[] GetAllEnterprises(string token)
        {
            if (CheckToken(token) == null)
                return null;
            return this.GetEnterpriseFactory().GetEnterprises();
        }

    }
}
