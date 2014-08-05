using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Trasys.Dev.Tools.Data.Annotations;

namespace Trasys.PhotoTrack.Api.Models
{
    [DataContract]
    public class Enterprise
    {
        [DataMember]
        [Column("EnterpriseID")]
        public long EnterpriseID { get; set; }

        [DataMember]
        [Column("Name")]
        public string Name { get; set; }
    }
}