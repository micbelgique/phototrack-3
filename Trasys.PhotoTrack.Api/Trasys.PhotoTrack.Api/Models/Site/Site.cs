using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Trasys.Dev.Tools.Data.Annotations;

namespace Trasys.PhotoTrack.Api.Models
{
    [DataContract]
    public class Site
    {
        [DataMember]
        [Column("SiteID")]
        public long SiteID { get; set; }

        [DataMember]
        [Column("Number")]
        public string Number { get; set; }

        [DataMember]
        [Column("Adress")]
        public string Address { get; set; }

        [DataMember]
        [Column("Description")]
        public string Description { get; set; }

        [DataMember]
        [Column("EnterpriseName")]
        public string EnterpriseName { get; set; }

        [DataMember]
        [Column("EnterpriseID")]
        public long EnterpriseID { get; set; }

        [DataMember]
        [Column("Status")]
        public string Status { get; set; }

        [DataMember]
        [Column("Latitude")]
        public double Latitude { get; set; }

        [DataMember]
        [Column("Longitude")]
        public double Longitude { get; set; }

        [DataMember]
        [Column("PlannedDate")]
        public DateTime PlannedDate { get; set; }
    }
}