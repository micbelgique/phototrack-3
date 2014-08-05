using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Trasys.PhotoTrack.Api.Models
{
    [DataContract]
    public class Photo
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public string Tag { get; set; }
        [DataMember]
        public DateTime Timestamp { get; set; }
    }
}