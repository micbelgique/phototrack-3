using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Trasys.Dev.Tools.Data.Annotations;

namespace Trasys.PhotoTrack.Api.Models
{
    [DataContract]
    public class SiteDetails : Site
    {
        [DataMember]
        [NotMapped]
        public ProfileSummary[] Holders { get; set; }
    }
}