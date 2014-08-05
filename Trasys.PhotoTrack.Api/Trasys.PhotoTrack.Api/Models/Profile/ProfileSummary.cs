using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Trasys.Dev.Tools.Data.Annotations;

namespace Trasys.PhotoTrack.Api.Models
{
    [DataContract]
    public class ProfileSummary
    {
        [DataMember]
        [Column("ProfileID")]
        public long ProfileID { get; set; }
        [DataMember]
        [Column("FirstName")]
        public string FirstName { get; set; }
        [DataMember]
        [Column("LastName")]
        public string LastName { get; set; }
    }
}